using CERS.Guidance;
using CERS.Model;
using CERS.Security;
using CERS.Validation;
using CERS.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using UPF;

namespace CERS
{
	public static class ModelValidationExtensionMethods
	{
		#region Fields

		private static object _TypeDataRegistryCacheLock = new object();
		private static Dictionary<Type, List<ModelAttributeCacheItem<DataRegistryMetadataAttribute>>> _TypeDataRegistryMetadataAttributeCache;

		#endregion Fields

		#region CreateGuidanceMessage Method

		public static EntityGuidanceMessage CreateEntityGuidanceMessage<TModel, TValue>( this TModel model, GuidanceMessageCode code, Expression<Func<TModel, TValue>> expression, GuidanceLevel level = GuidanceLevel.Advisory, params object[] formatArguments ) where TModel : IDDModelEntity
		{
			if ( expression == null )
			{
				throw new ArgumentNullException( "expression" );
			}

			var property = model.GetPropertyInfo( expression );
			if ( property == null )
			{
				throw new ArgumentException( "Property cannot be found from expression.", "expression" );
			}
			object propertyValue = property.GetValue( model, null );
			var result = EntityGuidanceMessage.Create( code, property.Name, propertyValue, level, model, formatArguments );
			return result;
		}

		#endregion CreateGuidanceMessage Method

		#region ValidateDataRegistryMetadata Method

		/// <summary>
		/// Validates all properties decorated with <see cref="DataRegistryMetadataAttribute" />
		/// against the metadata in CDR
		/// </summary>
		/// <typeparam name="TModel"></typeparam>
		/// <param name="entity"></param>
		/// <returns></returns>
		public static DataRegistryValidationResult<TModel> ValidateDataRegistryMetadata<TModel>( this TModel entity ) where TModel : class, IDDModelEntity
		{
			if ( entity == null )
			{
				throw new ArgumentNullException( "entity" );
			}

			//declare local storage variables for results and other stuff.
			Type entityType = typeof( TModel );
			int? resourceTypeID = null;
			int minimallyRequiredFieldTotalCount = 0;
			int minimallyRequiredFieldsSatisfiedCount = 0;

			//declare the guidance messages to be returned.
			DataRegistryValidationResult<TModel> results = new DataRegistryValidationResult<TModel>( entity );

			//see if we can get a handle on the FacilitySubmittalElementResourceEntityAttribute attribute.
			var fserEntityAttribute = ModelMetadataHelper.GetTypeAttribute<FacilitySubmittalElementResourceEntityAttribute>( entityType );

			//if we have the attribute, lets get the entities ResourceType.
			if ( fserEntityAttribute != null )
			{
				resourceTypeID = (int)fserEntityAttribute.Type;
			}

			//get the properties for the entity.
			var properties = ModelMetadataHelper.GetPropertyMetadataAttributesForType<TModel, DataRegistryMetadataAttribute>( ref _TypeDataRegistryCacheLock, ref _TypeDataRegistryMetadataAttributeCache );

			// If the Entity Type is BPFacilityChemical, cast entity to an instance of
			// BPFacilityChemical for use in any generated Guidance Messages:
			BPFacilityChemical bpFacilityChemical = null;
			if ( entityType == typeof( BPFacilityChemical ) )
			{
				bpFacilityChemical = (BPFacilityChemical)(object)entity;
			}

			//declare local storage variables for loop iteration processing.
			string propertyDisplayName = null;
			string propertyName = null;
			object propertyValue = null;
			string value = null;
			IDataElementItem dataElement = null;
			List<string> dataElementCodes = null;
			bool fieldIsValid = false;

			foreach ( var property in properties )
			{
				//get the DataRegistry Data from CDR.
				propertyName = property.ModelProperty.Name;
				propertyValue = property.ModelProperty.GetValue( entity );
				propertyDisplayName = string.Empty;
				dataElement = property.Tag as IDataElementItem;
				fieldIsValid = true;

				if ( property.Tag == null )
				{
					dataElement = DataRegistry.GetDataElement( property.Attribute );

					//lets put this dataElement get just got and attach it to the property in cache so we have a few
					//CPU cycles the next time we process this same Type.
					property.Tag = dataElement;
				}

				if ( !property.Attribute.IgnoreValidation )
				{
					//only process further if we were able to obtain a DataElement from above.
					if ( dataElement != null )
					{
						propertyDisplayName = dataElement.FieldLabelText ?? dataElement.FieldName;
						if ( dataElement.CERSMinRequired )
						{
							//we know this field is a minimally required field.
							minimallyRequiredFieldTotalCount++;
						}

						//see if the originalValue field is null.
						if ( propertyValue != null )
						{
							//if we can convert it to a string lets do so. we need to be able to validate only using strings (generically anyway) so this is a definiate condition that needs to be met.
							if ( property.ModelProperty.Converter.CanConvertTo( typeof( string ) ) )
							{
								//convert it to a string.
								value = ( (string)property.ModelProperty.Converter.ConvertTo( propertyValue, typeof( string ) ) ).Trim();

								if ( !string.IsNullOrWhiteSpace( value ) )
								{
									//check for some codes to validate against.
									if ( dataElement.Codes.Count() > 0 )
									{
										// If there are codes, and the propertyValue is of type
										// Decimal, strip the trailing zeroes and decimal before
										// validating the code against the list.
										// ** This may be obsolete, as DOTHazClassID has changed
										//    from Decimal to String as of 3/9/2012
										if ( propertyValue.GetType() == System.Type.GetType( "System.Decimal" ) )
										{
											value = ( (Decimal)propertyValue ).ToString( "G29" );
										}

										//we have some codes, so lets verify the values against the list.
										//some fields may contain multiple (select one or many type fields) values delimitted by semicolons.
										//we only care about the codes for validating, not the code description's, so transform into a generic list.
										dataElementCodes = dataElement.Codes.Select( c => c.Code ).ToList();

										if ( !string.IsNullOrWhiteSpace( value ) )
										{
											//use custom extension to split the field by the expected
											//delimitter, then compare against the codes (deals with mulidimmenisal values)
											if ( !value.IfInRange( ";", dataElementCodes.ToArray() ) )
											{
												//value for field is outside the range of valid values, so lets write a guidance message
												if ( entityType == typeof( BPFacilityChemical ) && bpFacilityChemical != null )
												{
													// Custom HMI Value Outside of Acceptable Range
													// Error for Hazardous Material Inventory
													results.GuidanceMessages.Add( GuidanceMessageCode.HMIValueOutsideAcceptableRange, propertyName, value, GuidanceLevel.Required, null, value, propertyDisplayName, bpFacilityChemical.CommonName, bpFacilityChemical.ChemicalLocation );
												}
												else
												{
													results.GuidanceMessages.Add( GuidanceMessageCode.ValueOutsideAcceptableRange, propertyName, value, GuidanceLevel.Required, null, resourceTypeID );
												}
												fieldIsValid = false;
											}
										}
									}

									//see if there is a regular expression defined and if so, lets validate against it.
									if ( !string.IsNullOrWhiteSpace( dataElement.ValidationRegularExpression ) )
									{
										//there is so lets make sure we are dealing with a string.
										string stringValue = propertyValue.ToString();

										//validate against regular expression.
										if ( !dataElement.ValidateRegulatorExpression( stringValue ) )
										{
											//does not match so lets write guidance message.
											if ( entityType == typeof( BPFacilityChemical ) && bpFacilityChemical != null )
											{
												// Custom "HMI Field Value Does Not Validate Against
												// Regular Expression" for Hazardous Material Inventory
												results.GuidanceMessages.Add( GuidanceMessageCode.HMIFieldValueDoesNotValidateAgainstRegularExpression, property.ModelProperty.Name, stringValue, GuidanceLevel.Required, null, stringValue, propertyDisplayName, bpFacilityChemical.CommonName, bpFacilityChemical.ChemicalLocation, dataElement.FieldFormat );
											}
											else
											{
												results.GuidanceMessages.Add( GuidanceMessageCode.FieldValueDoesNotValidateAgainstRegularExpression, property.ModelProperty.Name, stringValue, GuidanceLevel.Required, null, resourceTypeID, propertyDisplayName, dataElement.FieldFormat );
											}
										}
									}
								}
								else
								{
									if ( dataElement.CERSMinRequired )
									{
										if ( entityType == typeof( BPFacilityChemical ) && bpFacilityChemical != null )
										{
											// Custom "HMI Minimally Required Field Does Not Have
											// Value" for Hazardous Material Inventory
											results.GuidanceMessages.Add( GuidanceMessageCode.HMIMinimallyRequiredFieldDoesNotHaveValue, propertyName, propertyValue, GuidanceLevel.Required, entity, propertyDisplayName, bpFacilityChemical.CommonName, bpFacilityChemical.ChemicalLocation );
										}
										else
										{
											results.GuidanceMessages.Add( GuidanceMessageCode.MinimallyRequiredFieldDoesNotHaveValue, propertyName, propertyValue, GuidanceLevel.Required, entity, propertyDisplayName );
										}
										fieldIsValid = false;
									}
								}
							}
						}
						else
						{
							//check tp see if its a minimally required field, if it is, then write message indicating it's not populated.
							if ( dataElement.CERSMinRequired )
							{
								if ( entityType == typeof( BPFacilityChemical ) && bpFacilityChemical != null )
								{
									// Custom "HMI Minimally Required Field Does Not Have Value" for
									// Hazardous Material Inventory
									results.GuidanceMessages.Add( GuidanceMessageCode.HMIMinimallyRequiredFieldDoesNotHaveValue, propertyName, propertyValue, GuidanceLevel.Required, entity, propertyDisplayName, bpFacilityChemical.CommonName, bpFacilityChemical.ChemicalLocation );
								}
								else
								{
									results.GuidanceMessages.Add( GuidanceMessageCode.MinimallyRequiredFieldDoesNotHaveValue, propertyName, propertyValue, GuidanceLevel.Required, entity, propertyDisplayName );
								}
								fieldIsValid = false;
							}
						}

						if ( dataElement.CERSMinRequired && fieldIsValid )
						{
							//incriment the statisfied count
							minimallyRequiredFieldsSatisfiedCount++;
						}
					}
				}
			}

			return results;
		}

		#endregion ValidateDataRegistryMetadata Method

		#region Validate Methods

		public static void FacInfoValidate( DataTable uploadedDataTable, int rowDataBegins, List<GuidanceMessage> guidanceMessages, LUTGuidanceLevel lutGuidanceLevel, int guidanceMessageTruncationLimit, int organizationID )
		{
			// Reset rowIndex to the first row of data
			int rowIndex = rowDataBegins;
			var repository = ServiceLocator.GetRepositoryManager();

			// Use CDR to Determine Minimally Required Fields for Facility Information Upload
			var cdr = from c in CERS.DataRegistry.GetDataElements( DataRegistryDataSourceType.UPDD )
					  where /*c.TableName == "Business Activities" || */c.TableName == "Business Owner / Operator Identification"
					  select c;

			// Validate that all Minimally-Required Fields are populated
			foreach ( DataRow row in uploadedDataTable.Rows )
			{
				if ( !string.IsNullOrWhiteSpace( row.ItemArray.ToDelimitedString( "" ) ) )
				{
					#region XML Validation

					if ( !row.ItemArray.ToDelimitedString( "" ).IsValidXmlString() )
					{
						for ( int i = 0; i < row.ItemArray.Length; i++ )
						{
							if ( !row.ItemArray[i].ToString().IsValidXmlString() )
							{
								repository.GuidanceMessages.AddGuidanceMessage( guidanceMessages, lutGuidanceLevel, String.Format( "{0} contains illegal character(s) (row {1}).", row.Table.Columns[i].ColumnName, rowIndex ) );
							}
						}
					}

					#endregion XML Validation

					#region CERSID Validation

					if ( row["CERSID"] == null || row["CERSID"].ToString().Equals( "" ) )
					{
						repository.GuidanceMessages.AddGuidanceMessage( guidanceMessages, lutGuidanceLevel, String.Format( "CERSID must be specified (row {0}).", rowIndex ) );
					}
					else if ( repository.Facilities.GridSearch( CERSID: row["CERSID"].ToString().ToInt32(), organizationID: organizationID ).Count() == 0 )
					{
						repository.GuidanceMessages.AddGuidanceMessage( guidanceMessages, lutGuidanceLevel, String.Format( "CERSID# {0} does not belong to this organization (row {1}).", row["CERSID"], rowIndex ) );
					}

					#endregion CERSID Validation

					#region CDR validation

					foreach ( var e in cdr )
					{
						if ( !string.IsNullOrWhiteSpace( e.XmlTagName ) && row.Table.Columns.Contains( e.XmlTagName ) && row[e.XmlTagName] != null )
						{
							if ( e.CERSMinRequired && String.IsNullOrWhiteSpace( row[e.XmlTagName].ToString() ) )
							{
								repository.GuidanceMessages.AddGuidanceMessage( guidanceMessages, lutGuidanceLevel, String.Format( "{0} must be specified (row {1}).", e.XmlTagName, rowIndex ) );
							}
							else if ( row[e.XmlTagName].ToString().Length > e.DataLength && e.UIElementType == UIElementType.TextBox )
							{
								repository.GuidanceMessages.AddGuidanceMessage( guidanceMessages, lutGuidanceLevel, String.Format( "Field {0} value is too large maximum length is {1} (row {2}).", e.XmlTagName, e.DataLength, rowIndex ) );
							}
							else if ( e.Codes.Count() > 0 && !e.Codes.Select( c => c.Code ).ToList().Contains( row[e.XmlTagName].ToString() ) )
							{
                                repository.GuidanceMessages.AddGuidanceMessage( guidanceMessages, lutGuidanceLevel, String.Format( "Field {0} contain invalid value '{1}' (row {2}).", e.XmlTagName, row[e.XmlTagName].ToString(), rowIndex ) );
							}
						}

						//[ak 04/25/2013] do non-text base data type validation as well if we can help user from their own mistake
						if ( !String.IsNullOrWhiteSpace( row[e.XmlTagName].ToString() ) && ( e.DataType == "decimal" || e.DataType == "float" || e.DataType == "currency" ) )
						{
							Double dValue;
							if ( !Double.TryParse( row[e.XmlTagName].ToString().Trim(), out dValue ) )
							{
                                repository.GuidanceMessages.AddGuidanceMessage( guidanceMessages, lutGuidanceLevel, String.Format( "Field {0} contain invalid value '{1}' (row {2}).", e.XmlTagName, row[e.XmlTagName].ToString(), rowIndex ) );
							}
						}

						if ( !String.IsNullOrWhiteSpace( row[e.XmlTagName].ToString() ) && ( e.DataType == "int" || e.DataType == "smallint" || e.DataType == "tinyint" ) )
						{
							int iValue;
							if ( !int.TryParse( row[e.XmlTagName].ToString().Trim(), out iValue ) )
							{
                                repository.GuidanceMessages.AddGuidanceMessage( guidanceMessages, lutGuidanceLevel, String.Format( "Field {0} contain invalid value '{1}' (row {2}).", e.XmlTagName, row[e.XmlTagName].ToString(), rowIndex ) );
							}
						}

						if ( !String.IsNullOrWhiteSpace( row[e.XmlTagName].ToString() ) && ( e.DataType == "date" || e.DataType == "smalldatetime" || e.DataType == "datetime" || e.DataType == "time" ) )
						{
							DateTime dtValue;
							if ( !DateTime.TryParse( row[e.XmlTagName].ToString().Trim(), out dtValue ) )
							{
                                repository.GuidanceMessages.AddGuidanceMessage( guidanceMessages, lutGuidanceLevel, String.Format( "Field {0} contain invalid value '{1}' (row {2}).", e.XmlTagName, row[e.XmlTagName].ToString(), rowIndex ) );
							}
						}
					}

					#endregion CDR validation
				}
				rowIndex++;

				// Break out of validation loop if we have found a number of errors exceeding the guidanceMessageTruncationLimit
				if ( guidanceMessages.Count > guidanceMessageTruncationLimit )
				{
					// Truncate any guidanceMessages over the guidanceMessageTruncationLimit
					guidanceMessages.RemoveRange( guidanceMessageTruncationLimit, guidanceMessages.Count - guidanceMessageTruncationLimit );
					break;
				}
			}
		}

		public static int HMIValidate( DataTable uploadedDataTable, int rowDataBegins, List<GuidanceMessage> guidanceMessages, LUTGuidanceLevel lutGuidanceLevel, int guidanceMessageTruncationLimit, int organizationID, int? CERSID )
		{
			int rowCountForCurrentCERSID = 0;
			// Reset rowIndex to the first row of data
			int rowIndex = rowDataBegins;
			int rowWithData = 0;

			var repository = ServiceLocator.GetRepositoryManager();

			// Use CDR to Determine Minimally Required Fields for Hazardous Material Inventory Upload
			var cdr = from c in CERS.DataRegistry.GetDataElements( DataRegistryDataSourceType.UPDD ) where c.TableName == "Hazardous Materials Inventory - Chemical Description" select c;

			// Validate that all Minimally-Required Fields are populated
			foreach ( DataRow row in uploadedDataTable.Rows )
			{
				#region XML Validation

				if ( !row.ItemArray.ToDelimitedString( "" ).IsValidXmlString() )
				{
					for ( int i = 0; i < row.ItemArray.Length; i++ )
					{
						if ( !row.ItemArray[i].ToString().IsValidXmlString() )
						{
							repository.GuidanceMessages.AddGuidanceMessage( guidanceMessages, lutGuidanceLevel, String.Format( "{0} contains illegal character(s) (row {1}).", row.Table.Columns[i].ColumnName, rowIndex ) );
						}
					}
				}

				#endregion XML Validation

				#region CERSID Validation

				if ( row["CERSID"] == null || row["CERSID"].ToString().Equals( "" ) )
				{
					if ( !( row["CommonName"] == null || row["CommonName"].ToString().Trim().Equals( "" ) ) )
					{
						repository.GuidanceMessages.AddGuidanceMessage( guidanceMessages, lutGuidanceLevel, String.Format( "CERSID must be specified (row {0}).", rowIndex ) );
					}
					else
					{
						continue;
					}
				}
				else if ( CERSID.HasValue && !row["CERSID"].ToString().Equals( CERSID.ToString() ) )
				{
					// If we end up in this block, this means that the uploaded spreadsheet had
					// CERSID values that do not match the current facility's CERS ID. Add a
					// Guidance Message accordingly -
					repository.GuidanceMessages.AddGuidanceMessage( guidanceMessages, lutGuidanceLevel, String.Format( "CERSID in uploaded file ({0}) does not match current facility's CERS ID ({1}) (row {2}).", row["CERSID"], CERSID, rowIndex ) );
				}
				else if ( repository.Facilities.GridSearch( CERSID: row["CERSID"].ToString().ToInt32(), organizationID: organizationID ).Count() == 0 )
				{
					repository.GuidanceMessages.AddGuidanceMessage( guidanceMessages, lutGuidanceLevel, String.Format( "CERSID# {0} does not belong to this organization (row {1}).", row["CERSID"], rowIndex ) );
				}

				#endregion CERSID Validation

				rowCountForCurrentCERSID++;

				#region PercentByWeight Validation

				bool outOfRange = false;
				string fieldsPctByWeight = string.Empty;
				for ( int i = 1; i <= 5; i++ )
				{
					string fieldName = "HC" + i.ToString() + "PercentByWeight";
					string strPctByWeight = row[fieldName].ToString().Trim();
					if ( !strPctByWeight.IsNullOrWhiteSpace() )
					{
						Double pctByWeight;
						double.TryParse( strPctByWeight, out pctByWeight );

						if ( pctByWeight < .01D || pctByWeight > 100.00D )
						{
							outOfRange = true;
							fieldsPctByWeight += ( string.IsNullOrWhiteSpace( fieldsPctByWeight ) ? "" : ", " ) + fieldName;
						}
					}
				}
				if ( outOfRange )
				{
					repository.GuidanceMessages.AddGuidanceMessage( guidanceMessages, lutGuidanceLevel, "Field" + ( fieldsPctByWeight.Contains( "," ) ? "s " : " " ) + fieldsPctByWeight + " Percentage By Weight " + ( fieldsPctByWeight.Contains( "," ) ? "are" : "is" ) + " out of range (row " + rowIndex + ")." );
				}

				#endregion PercentByWeight Validation

				#region CDR validation

				foreach ( var e in cdr )
				{
					if ( !string.IsNullOrWhiteSpace( e.XmlTagName ) && row.Table.Columns.Contains( e.XmlTagName ) && row[e.XmlTagName] != null )
					{
						if ( e.CERSMinRequired && String.IsNullOrWhiteSpace( row[e.XmlTagName].ToString() ) )
						{
							repository.GuidanceMessages.AddGuidanceMessage( guidanceMessages, lutGuidanceLevel, String.Format( "{0} must be specified (row {1}).", e.FieldName, rowIndex ) );
						}

                        if ( row[e.XmlTagName].ToString().Trim().Length > e.DataLength )
						{
							if ( e.Codes.Count() > 0 )
							{
                                repository.GuidanceMessages.AddGuidanceMessage( guidanceMessages, lutGuidanceLevel, String.Format( "Field {0} contain invalid value '{1}' (row {2}).", e.XmlTagName, row[e.XmlTagName].ToString(), rowIndex ) );
							}
							else
							{
								repository.GuidanceMessages.AddGuidanceMessage( guidanceMessages, lutGuidanceLevel, String.Format( "Field {0} value is too large maximum length is {1} (row {2}).", e.XmlTagName, e.DataLength, rowIndex ) );
							}
						}

						//[ak 04/25/2013] do non-text base data type validation as well if we can help user from their own mistake
						if ( !String.IsNullOrWhiteSpace( row[e.XmlTagName].ToString() ) && ( e.DataType == "decimal" || e.DataType == "float" || e.DataType == "currency" ) )
						{
							Double dValue;
							if ( !Double.TryParse( row[e.XmlTagName].ToString().Trim(), out dValue ) )
							{
                                repository.GuidanceMessages.AddGuidanceMessage( guidanceMessages, lutGuidanceLevel, String.Format( "Field {0} contain invalid value '{1}' (row {2}).", e.XmlTagName, row[e.XmlTagName].ToString(), rowIndex ) );
							}
						}

						if ( !String.IsNullOrWhiteSpace( row[e.XmlTagName].ToString() ) && ( e.DataType == "int" || e.DataType == "smallint" || e.DataType == "tinyint" ) )
						{
							int iValue;
							if ( !int.TryParse( row[e.XmlTagName].ToString().Trim(), out iValue ) )
							{
                                repository.GuidanceMessages.AddGuidanceMessage( guidanceMessages, lutGuidanceLevel, String.Format( "Field {0} contain invalid value '{1}' (row {2}).", e.XmlTagName, row[e.XmlTagName].ToString(), rowIndex ) );
							}
						}

						if ( !String.IsNullOrWhiteSpace( row[e.XmlTagName].ToString() ) && ( e.DataType == "date" || e.DataType == "smalldatetime" || e.DataType == "datetime" || e.DataType == "time" ) )
						{
							DateTime dtValue;
							if ( !DateTime.TryParse( row[e.XmlTagName].ToString().Trim(), out dtValue ) )
							{
                                repository.GuidanceMessages.AddGuidanceMessage( guidanceMessages, lutGuidanceLevel, String.Format( "Field {0} contain invalid value '{1}' (row {2}).", e.XmlTagName, row[e.XmlTagName].ToString(), rowIndex ) );
							}
						}
					}
				}

				#endregion CDR validation

				rowIndex++;

				// Break out of validation loop if we have found a number of errors exceeding the guidanceMessageTruncationLimit
				if ( guidanceMessages.Count > guidanceMessageTruncationLimit )
				{
					// Truncate any guidanceMessages over the guidanceMessageTruncationLimit
					guidanceMessages.RemoveRange( guidanceMessageTruncationLimit, guidanceMessages.Count - guidanceMessageTruncationLimit );
					break;
				}

				#region Data row counter

				if ( row["CERSID"] != null && !row["CERSID"].ToString().Equals( "" )
					//&& row["ChemicalName"] != null && !row["ChemicalName"].ToString().Equals( "" )
					&& row["CommonName"] != null && !row["CommonName"].ToString().Equals( "" ) )
				{
					rowWithData++;
				}

				#endregion Data row counter
			}

			// If zero rows were found matching the current Facility's CERSID, add appropriate
			// Guidance Message
			if ( rowCountForCurrentCERSID == 0 && CERSID.HasValue )
			{
				repository.GuidanceMessages.AddGuidanceMessage( guidanceMessages, lutGuidanceLevel, "No CERS ID values in the uploaded spreadsheet match the current facility's CERS ID (" + CERSID.ToString() + ")." );
			}

			return rowWithData;
		}

		public static CMEPackageValidationResult Validate( this CMEPackage cmePackage, ICERSRepositoryManager repository, CallerContext callerContext = CallerContext.UI, PermissionRoleMatrixCollection permissions = null )
		{
			if ( cmePackage == null )
			{
				throw new ArgumentNullException( "cmePackage" );
			}

			if ( repository == null )
			{
				throw new ArgumentNullException( "repository" );
			}

			CMEPackageValidator validator = new CMEPackageValidator();
			return validator.Validate( cmePackage, repository, callerContext, permissions );
		}

		#region ICMEModelEntity Validate Methods

		public static ICMEModelEntityValidationResult<TModel> ValidateCME<TModel>( this TModel entity, ICERSRepositoryManager repository, CallerContext context = CallerContext.UI, PermissionRoleMatrixCollection permissions = null ) where TModel : class, ICMEModelEntity
		{
			if ( entity == null )
			{
				throw new ArgumentNullException( "entity" );
			}

			if ( repository == null )
			{
				throw new ArgumentNullException( "repository" );
			}

			var validator = ValidatorFactory.CreateForCMEModelEntity<TModel>();
			if ( validator == null )
			{
				throw new Exception( "Unable to find validator for " + typeof( TModel ).Name + "." );
			}
			validator.Initialize( repository, context, permissions );
			return entity.ValidateCME( validator, permissions );
		}

		public static ICMEModelEntityValidationResult<TModel> ValidateCME<TModel>( this TModel entity, ICMEModelEntityValidator<TModel, ICMEModelEntityValidationResult<TModel>> validator, PermissionRoleMatrixCollection permissions = null ) where TModel : class, ICMEModelEntity
		{
			if ( entity == null )
			{
				throw new ArgumentNullException( "entity" );
			}

			if ( validator == null )
			{
				throw new ArgumentNullException( "validator" );
			}
			return validator.Validate( entity, permissions );
		}

		#endregion ICMEModelEntity Validate Methods

		#region IDDModelEntity Validate Methods

		public static IDDModelEntityValidationResult<TModel> Validate<TModel>( this TModel entity, ICERSRepositoryManager repository, CallerContext context = CallerContext.UI, PermissionRoleMatrixCollection permissions = null ) where TModel : class, IDDModelEntity
		{
			if ( entity == null )
			{
				throw new ArgumentNullException( "entity" );
			}

			if ( repository == null )
			{
				throw new ArgumentNullException( "repository" );
			}

			var validator = ValidatorFactory.CreateForModelEntity<TModel>();
			if ( validator == null )
			{
				throw new Exception( "Unable to get the Validator for the type " + typeof( TModel ).Name + "." );
			}
			validator.Initialize( repository, context, permissions );
			return entity.Validate( validator, permissions );
		}

		public static IDDModelEntityValidationResult<TModel> Validate<TModel>( this TModel entity, IDDModelEntityValidator<TModel, IDDModelEntityValidationResult<TModel>> validator, PermissionRoleMatrixCollection permissions = null ) where TModel : class, IDDModelEntity
		{
			if ( entity == null )
			{
				throw new ArgumentNullException( "entity" );
			}

			if ( validator == null )
			{
				throw new ArgumentNullException( "validator" );
			}

			return validator.Validate( entity, permissions );
		}

		#endregion IDDModelEntity Validate Methods

		#region FacilitySubmittalElement Validate Methods

		public static IFacilitySubmittalElementValidationResult Validate( this FacilitySubmittalElement element, ICERSRepositoryManager repository, CallerContext context = CallerContext.UI, PermissionRoleMatrixCollection permissions = null )
		{
			if ( element == null )
			{
				throw new ArgumentNullException( "element" );
			}

			IFacilitySubmittalElementValidator validator = ValidatorFactory.CreateForFacilitySubmittalElement( element );
			if ( validator == null )
			{
				throw new Exception( "Unable to get the validator for the FacilitySubmittalElement." );
			}
			validator.Initialize( repository, context, permissions );
			return element.Validate( validator, permissions );
		}

		public static IFacilitySubmittalElementValidationResult Validate( this FacilitySubmittalElement element, IFacilitySubmittalElementValidator validator, PermissionRoleMatrixCollection permissions = null )
		{
			if ( element == null )
			{
				throw new ArgumentNullException( "element" );
			}

			if ( validator == null )
			{
				throw new ArgumentNullException( "validator" );
			}

			return validator.Validate( element, permissions );
		}

		#endregion FacilitySubmittalElement Validate Methods

		#region Resource Validate Methods

		public static IFacilitySubmittalElementResourceValidationResult Validate( this FacilitySubmittalElementResource resource, ICERSRepositoryManager repository, CallerContext context = CallerContext.UI, PermissionRoleMatrixCollection permissions = null )
		{
			if ( resource == null )
			{
				throw new ArgumentNullException( "resource" );
			}

			if ( repository == null )
			{
				throw new ArgumentNullException( "repository" );
			}

			var validator = ValidatorFactory.CreateForFacilitySubmittalElementResource( resource );
			if ( validator == null )
			{
				throw new Exception( "Unable to create a validator for the FacilitySubmittalElementResource." );
			}
			validator.Initialize( repository, context, permissions );
			return validator.Validate( resource, repository, context, permissions );
		}

		public static IFacilitySubmittalElementResourceValidationResult Validate( this FacilitySubmittalElementResource resource, IFacilitySubmittalElementResourceValidator validator )
		{
			if ( resource == null )
			{
				throw new ArgumentNullException( "resource" );
			}
			if ( validator == null )
			{
				throw new ArgumentNullException( "validator" );
			}
			return validator.Validate( resource );
		}

		#endregion Resource Validate Methods

		#region ResourceDocument Validate Methods

		public static FacilitySubmittalElementResourceDocumentValidationResult Validate( this FacilitySubmittalElementResourceDocument resourceDocument, ICERSRepositoryManager repository, CallerContext context = CallerContext.UI, PermissionRoleMatrixCollection permissions = null )
		{
			if ( resourceDocument == null )
			{
				throw new ArgumentNullException( "resourceDocument" );
			}

			if ( repository == null )
			{
				throw new ArgumentNullException( "repository" );
			}

			var validator = ValidatorFactory.CreateForResourceDocument( resourceDocument );
			if ( validator == null )
			{
				throw new Exception( "Unable to create a validator for the FacilitySubmittalElementResourceDocument" );
			}
			validator.Initialize( repository, context, permissions );
			return resourceDocument.Validate( validator, permissions );
		}

		public static FacilitySubmittalElementResourceDocumentValidationResult Validate( this FacilitySubmittalElementResourceDocument entity, IFacilitySubmittalElementResourceDocumentValidator validator, PermissionRoleMatrixCollection permissions = null )
		{
			if ( entity == null )
			{
				throw new ArgumentNullException( "entity" );
			}

			if ( validator == null )
			{
				throw new ArgumentNullException( "validator" );
			}
			return validator.Validate( entity, permissions );
		}

		#endregion ResourceDocument Validate Methods

		#endregion Validate Methods

		#region Validate Properties

		public static bool IsValidCERSID( this int CERSID )
		{
			return ( CERSID > 10000000 && CERSID < 30000000 );
		}

		#endregion Validate Properties

		#region MergeGuidanceMessages Method

		public static void MergeGuidanceMessages( this IFacilitySubmittalElementValidationResult fseValidationResult )
		{
			//loop through each resource inventory item (this scans through all found FSERS effectively)
			foreach ( var resourceInventoryItem in fseValidationResult.ResourceInventory )
			{
				var fserValidationResult = resourceInventoryItem.ValidationResult;

				//merge guidance messages from fser validation result into top level.
				fseValidationResult.GuidanceMessages.Merge( fserValidationResult.GuidanceMessages, fse: fseValidationResult.Entity );
			}
		}

		public static void MergeGuidanceMessages( this IFacilitySubmittalElementResourceValidationResult fserValidationResult )
		{
			if ( fserValidationResult != null )
			{
				foreach ( var fserEntity in fserValidationResult.ValidationResults )
				{
					//fserEntity
					fserValidationResult.GuidanceMessages.Merge( fserEntity.GuidanceMessages, fser: fserValidationResult.Entity, resourceType: fserValidationResult.ResourceType, resourceEntityID: fserEntity.EntityID );
				}
			}
		}

		#endregion MergeGuidanceMessages Method

		#region ValidateAndCommitResults Methods

		public static void ValidateAndCommitResults<TModel>( this FacilitySubmittalElementResourceViewModel<TModel> viewModel, ICERSRepositoryManager repository, CallerContext callerContext = CallerContext.UI ) where TModel : class, IFacilitySubmittalModelEntity
		{
			if ( viewModel.FacilitySubmittalElement != null )
			{
				viewModel.FacilitySubmittalElement.ValidateAndCommitResults( repository, callerContext );
			}
		}

		public static void ValidateAndCommitResults( this FacilitySubmittalElement fse, ICERSRepositoryManager repository, CallerContext callerContext = CallerContext.UI )
		{
			var result = fse.Validate( repository, callerContext );
			result.CommitValidationResults( repository );
		}

		public static void ValidateAndCommitResults( this FacilitySubmittalElement fse, ICERSRepositoryManager repository, BPFacilityChemical bpFacilityChemical, CallerContext callerContext = CallerContext.UI )
		{
			var result = bpFacilityChemical.Validate( repository, callerContext );
			result.CommitValidationResults( repository );
		}

		public static void ValidateAndCommitResults( this ICMEModelEntity cmeEntity, ResourceType resourceType, ICERSRepositoryManager repository, CallerContext callerContext = CallerContext.UI )
		{
			switch ( resourceType )
			{
				case ResourceType.Inspection:
					var inspectionValidationResult = ( (Inspection)cmeEntity ).ValidateCME<Inspection>( repository, callerContext );
					inspectionValidationResult.CommitValidationResults( resourceType, repository );
					break;

				case ResourceType.Violation:
					var violationValidationResult = ( (Violation)cmeEntity ).ValidateCME<Violation>( repository, callerContext );
					violationValidationResult.CommitValidationResults( resourceType, repository );
					break;

				case ResourceType.Enforcement:
					var enforcementValidationResult = ( (Enforcement)cmeEntity ).ValidateCME<Enforcement>( repository, callerContext );
					enforcementValidationResult.CommitValidationResults( resourceType, repository );
					break;

				case ResourceType.EnforcementViolation:
					var enforcementViolationValidationResult = ( (EnforcementViolation)cmeEntity ).ValidateCME<EnforcementViolation>( repository, callerContext );
					enforcementViolationValidationResult.CommitValidationResults( resourceType, repository );
					break;
			}
		}

		#endregion ValidateAndCommitResults Methods

		#region CommitValidationResults Methods

		public static void CommitValidationResults( this IDDModelEntityValidationResult<BPFacilityChemical> validationResult, ICERSRepositoryManager repository )
		{
			//so here update guidance messages for this chemical.
			repository.GuidanceMessages.Update( validationResult );
			repository.FacilitySubmittalElementResources.Update( validationResult );
		}

		public static void CommitValidationResults( this IFacilitySubmittalElementValidationResult fseValidationResults, ICERSRepositoryManager repository, int? transactionID = null )
		{
			repository.GuidanceMessages.Update( fseValidationResults, transactionID: transactionID );
			repository.FacilitySubmittalElementResources.Update( fseValidationResults );
		}

		public static void CommitValidationResults( this ICMEModelEntityValidationResult cmeEntityValidationResults, ResourceType resourceType, ICERSRepositoryManager repository )
		{
			repository.GuidanceMessages.Update( cmeEntityValidationResults, resourceType );
			switch ( resourceType )
			{
				case ResourceType.Inspection:
					repository.Inspections.Save( ( (ICMEModelEntityValidationResult<Inspection>)cmeEntityValidationResults ).Entity );
					break;

				case ResourceType.Violation:
					repository.Violations.Save( ( (ICMEModelEntityValidationResult<Violation>)cmeEntityValidationResults ).Entity );
					break;

				case ResourceType.Enforcement:
					repository.Enforcements.Save( ( (ICMEModelEntityValidationResult<Enforcement>)cmeEntityValidationResults ).Entity );
					break;

				case ResourceType.EnforcementViolation:
					repository.EnforcementViolations.Save( ( (ICMEModelEntityValidationResult<EnforcementViolation>)cmeEntityValidationResults ).Entity );
					break;
			}
		}

		#endregion CommitValidationResults Methods

		#region GetLastSubmittalDelta

		/// <summary>
		/// Compares two object and filters out whatever is in the "ignore" params collection
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="current"></param>
		/// <param name="last"></param>
		/// <param name="ignore"></param>
		/// <returns></returns>
		public static LastSubmittalElementDelta GetLastSubmittalDelta<T>( this T current, T last, params string[] ignore ) where T : class
		{
			//Defaults the LastSUbmittalElementDelta to New
			LastSubmittalElementDelta delta = LastSubmittalElementDelta.New;

			//Both of the entities passed in must have values.
			if ( current != null && last != null )
			{
				Type type = typeof( T );
				List<string> ignoreList = new List<string>( ignore );

				//If no changes are found, "None" will be returned.
				delta = LastSubmittalElementDelta.None;

				//Filter out properties that have associations.
				foreach ( System.Reflection.PropertyInfo pi in type.GetProperties( System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance ).Where( f => !f.PropertyType.FullName.Contains( "CERS.Model" ) && !f.PropertyType.FullName.Contains( "System.Data" ) ) )
				{
					if ( !ignoreList.Contains( pi.Name ) )
					{
						object selfValue = type.GetProperty( pi.Name ).GetValue( current, null );
						object toValue = type.GetProperty( pi.Name ).GetValue( last, null );

						//If the two values are not equal, this entity has been changed.
						if ( selfValue != toValue && ( selfValue == null || !selfValue.Equals( toValue ) ) )
						{
							delta = LastSubmittalElementDelta.Changed;
						}
					}
				}
			}

			return delta;
		}

		#endregion GetLastSubmittalDelta
	}
}