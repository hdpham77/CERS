using CERS.Compositions;
using CERS.Guidance;
using CERS.Model;
using CERS.ViewModels.FacilityManagement;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Mail;
using System.Text;
using UPF;
using UPF.Core;
using UPF.Linq;

namespace CERS
{
	public static class TranslationExtensionMethods
	{
		public static void Add( this NameValueCollection collection, string key, DateTime? value )
		{
			collection.Add( key, ( value.HasValue ? value.Value.ToString() : "" ) );
		}

		public static void Add( this NameValueCollection collection, string key, int? value )
		{
			collection.Add( key, ( value.HasValue ? value.Value.ToString() : "" ) );
		}

		public static void Add( this NameValueCollection collection, string key, string value )
		{
			collection.Add( key, ( value != null ? value : "" ) );
		}

		public static bool Contains( this string content, TextMacro macro )
		{
			bool result = false;
			result = ( content.IndexOf( macro.Format() ) > -1 );
			return result;
		}

		public static string CreateMacroLink( this UPF.Core.SystemServices.UrlService urls, string content, TextMacro textMacro, SystemPortalType portalType, string pathAndQuery )
		{
			if ( content.Contains( textMacro ) )
			{
				var link = urls.CreateLink( portalType, pathAndQuery );
				content = content.Replace( textMacro, link.Url );
			}
			return content;
		}

		public static string Format( this TextMacro token )
		{
			return "$" + token + "$";
		}

		public static AddFacilityWizardStep GetCurrentStep( this AddFacilityWizardState wizardState )
		{
			return (AddFacilityWizardStep)wizardState.CurrentStepID;
		}

		public static EventDeliveryMechanism GetDeliveryMechanism( this Event theEvent )
		{
			return (EventDeliveryMechanism)theEvent.DeliveryMechanismID;
		}

		public static EventTypeCode GetEventTypeCode( this Event theEvent )
		{
			return (EventTypeCode)theEvent.TypeID;
		}

		public static EventPriority GetOrganizationEventPriority( this Event theEvent )
		{
			return (EventPriority)theEvent.OrganizationPriorityID;
		}

		public static EventPriority GetRegulatorEventPriority( this Event theEvent )
		{
			return (EventPriority)theEvent.RegulatorPriorityID;
		}

		public static AddFacilityWizardResult GetResult( this AddFacilityWizardState wizardState )
		{
			return (AddFacilityWizardResult)wizardState.ResultID;
		}

		public static EDTTransactionStatus GetStatus( this EDTTransaction transaction )
		{
			EDTTransactionStatus result = EDTTransactionStatus.Rejected;
			if ( transaction != null )
			{
				result = (EDTTransactionStatus)transaction.StatusID;
			}
			return result;
		}

		/// <summary>
		/// Gets a value indicating whether the <paramref name="schemaMetadata" /> is equal to
		/// <paramref name="schema" />.
		/// </summary>
		/// <param name="schemaMetadata">
		/// The <see cref="ISchemaMetadata" /> to compare against the <paramref name="schema" />
		/// </param>
		/// <param name="schema">
		/// The <see cref="XmlSchema" /> to compare the <paramref name="schemaMetadata" /> to.
		/// </param>
		/// <returns>
		/// Returns True if the <paramref name="schemaMetadata" /> and the <paramref name="schema"
		/// /> are equal.
		/// </returns>
		public static bool Is( this IXmlSchemaMetadata schemaMetadata, XmlSchema schema )
		{
			return ( schemaMetadata.XmlSchemaID == (int)schema );
		}

		/// <summary>
		/// Parses a string in the following format into a string key/value dictionary that follows
		/// the following pattern:
		/// Key: Value|Key:Value
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
		public static Dictionary<string, string> ParseMultiValueField( this string data )
		{
			Dictionary<string, string> parts = new Dictionary<string, string>();
			if ( !string.IsNullOrWhiteSpace( data ) )
			{
				string[] subParts = null;
				string[] rawParts = data.Split( new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries );

				foreach ( var rawPart in rawParts )
				{
					subParts = rawPart.Split( new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries );
					if ( subParts.Length >= 2 ) // assume colons after first colon are part of value string, e.g. time strings
					{
						string valueString = subParts[1];
						for ( int i = 2; i < subParts.Length; i++ )
						{
							valueString += ":" + subParts[i];
						}
						parts.Add( subParts[0], valueString );
					}
				}
			}
			return parts;
		}

		public static string Replace<T>( this string content, TextMacro macro, T value )
		{
			if ( content.Contains( macro ) )
			{
				string sanitizedValue = string.Empty;
				if ( value != null )
				{
					sanitizedValue = value.ToString();
				}
				return content.Replace( macro.Format(), sanitizedValue );
			}
			else
			{
				return content;
			}
		}

		public static void SetResult( this AddFacilityWizardState wizardState, AddFacilityWizardResult result )
		{
			wizardState.ResultID = (int)result;
		}

		public static void SetResult( this EDTAuthenticationRequest request, EDTAuthenticationRequestResult result )
		{
			request.ResultID = (int)result;
		}

		public static void SetStep( this AddFacilityWizardViewModel viewModel, AddFacilityWizardStep step )
		{
			viewModel.State.SetStep( step );
			viewModel.TargetStep = step;
		}

		public static void SetStep( this AddFacilityWizardState state, AddFacilityWizardStep step )
		{
			state.CurrentStepID = (int)step;
		}

		public static bool StatusIs( this Email email, EmailStatus status )
		{
			return email.StatusID == (int)status;
		}

		public static string ToDelimitedString( this IEnumerable<EDTTransactionMessage> messages, EDTTransactionMessageType? messageType = null, string delimitter = "," )
		{
			string result = null;
			if ( messages != null )
			{
				if ( messageType.HasValue )
				{
					var list = messages.Where( p => p.TypeID == (int)messageType.Value );
					if ( list.Count() > 0 )
					{
						result = list.Select( p => p.Message ).ToDelimitedString( delimitter );
					}
				}
				else
				{
					result = messages.Select( p => p.Message ).ToDelimitedString( delimitter );
				}
			}
			return result;
		}

		public static IEnumerable<RegulatorItem> ToSimpleItemView( this IEnumerable<Regulator> regulators )
		{
			var results = from r in regulators
						  select r.ToItem();

			return results;
		}

		#region ToItem Methods

		public static SubmittalElementItem ToItem( this SubmittalElement submittalElement )
		{
			var item = new SubmittalElementItem
			{
				ID = submittalElement.ID,
				Name = submittalElement.Name,
				Acronym = submittalElement.Acronym
			};
			return item;
		}

		public static RegulatorItem ToItem( this Regulator regulator, bool preferred = false )
		{
			var item = new RegulatorItem
			{
				ID = regulator.ID,
				Name = regulator.Name,
				TypeCode = regulator.Type.TypeCode,
				TypeID = regulator.TypeID,
				TypeName = regulator.Type.Name,
				Preferred = preferred
			};
			return item;
		}

		#endregion ToItem Methods

		#region Fields

		private static Dictionary<Type, List<ModelAttributeCacheItem<DataRegistryMetadataAttribute, IDataElementItem>>> _Cache;
		private static object _Lock = new object();

		#endregion Fields

		#region Internal GetDataRegistryProperties Method(s)

		internal static List<ModelAttributeCacheItem<DataRegistryMetadataAttribute, IDataElementItem>> GetDataRegistryProperties<TEntity>() where TEntity : class
		{
			Type TEntityType = typeof( TEntity );
			if ( _Cache == null )
			{
				lock ( _Lock )
				{
					if ( _Cache == null ) // check again once we have the lock
					{
						_Cache = new Dictionary<Type, List<ModelAttributeCacheItem<DataRegistryMetadataAttribute, IDataElementItem>>>();
					}
				}
			}

			List<ModelAttributeCacheItem<DataRegistryMetadataAttribute, IDataElementItem>> typeAttributes = null;
			if ( !_Cache.ContainsKey( TEntityType ) )
			{
				lock ( _Lock )
				{
					if ( !_Cache.ContainsKey( TEntityType ) ) // check inside the lock in case another thread changed the cache
					{
						var basePropertyAttributes = ModelMetadataHelper.GetPropertyMetadataAttributesForType<TEntity, DataRegistryMetadataAttribute>();
						typeAttributes = ( from r in basePropertyAttributes
										   let metadata = DataRegistry.GetDataElement( r.Attribute )
										   where metadata != null
										   select new ModelAttributeCacheItem<DataRegistryMetadataAttribute, IDataElementItem>
										   {
											   Attribute = r.Attribute,
											   BuddyProperty = r.BuddyProperty,
											   ModelProperty = r.ModelProperty,
											   Tag = r.Tag,
											   Metadata = metadata
										   } ).ToList();

						_Cache.Add( TEntityType, typeAttributes );
					}
				}
			}
			return _Cache[TEntityType];
		}

		#endregion Internal GetDataRegistryProperties Method(s)

		#region Public TranslateDataRegistryProperties Methods

		/// <summary>
		/// Looks for properties with the <see cref="DataRegistryMetadataAttribute" /> on
		/// <typeparamref name="TEntity" /> and if any codes are associated with the Data Registry
		/// metadata the property values are replaced with the description for the respective code.
		/// </summary>
		/// <typeparam name="TEntity">Any class</typeparam>
		/// <param name="items">A generic list of <typeparamref name="TEntity" /> items.</param>
		public static void TranslateDataRegistryProperties<TEntity>( this List<TEntity> items ) where TEntity : class
		{
			var typeAttributes = GetDataRegistryProperties<TEntity>();
			if ( typeAttributes != null )
			{
				foreach ( var gridViewItem in items )
				{
					TranslateDataRegistryProperties( gridViewItem, typeAttributes );
				}
			}
		}

		/// <summary>
		/// Looks for properties with the <see cref="DataRegistryMetadataAttribute" /> on
		/// <typeparamref name="TEntity" /> and if any codes are associated with the Data Registry
		/// metadata the property values are replaced with the description for the respective code.
		/// </summary>
		/// <typeparam name="TEntity">Any class</typeparam>
		/// <param name="item">
		/// A <typeparamref name="TEntity" /> item to translate property values for.
		/// </param>
		public static void TranslateDataRegistryProperties<TEntity>( this TEntity item ) where TEntity : class
		{
			var typeAttributes = GetDataRegistryProperties<TEntity>();
			if ( typeAttributes != null && typeAttributes.Count > 0 )
			{
				// Added "typeAttributes" parameter to stop infinite-loop
				TranslateDataRegistryProperties( item, typeAttributes );
			}
		}

		#endregion Public TranslateDataRegistryProperties Methods

		#region Private TranslateDataRegistryProperties(s)

		private static void TranslateDataRegistryProperties<TEntity>( this TEntity item, List<ModelAttributeCacheItem<DataRegistryMetadataAttribute, IDataElementItem>> typeProperties ) where TEntity : class
		{
			if ( item == null )
			{
				throw new ArgumentNullException( "gridViewItem" );
			}
			if ( typeProperties != null )
			{
				foreach ( var typeProperty in typeProperties )
				{
					var metadata = typeProperty.Metadata;
					if ( metadata != null )
					{
						if ( metadata.Codes.Count() > 0 )
						{
							object existingCode = typeProperty.ModelProperty.GetValue( item );
							if ( existingCode != null )
							{
								//get the description
								var code = metadata.Codes.SingleOrDefault( p => p.Code == existingCode.ToString() );
								if ( code != null )
								{
									typeProperty.ModelProperty.SetValue( item, code.Description );
								}
							}
						}
					}
				}
			}
		}

		#endregion Private TranslateDataRegistryProperties(s)

		#region ToLookup Method(s)

		public static IEnumerable<IIDNameLookupEntity> ToLookup( this IEnumerable<Regulator> regulators )
		{
			if ( regulators == null )
			{
				throw new ArgumentNullException( "regulators" );
			}
			List<IIDNameLookupEntity> results = new List<IIDNameLookupEntity>();
			results.AddRange( from r in regulators
							  select new IDNameLookupEntity
							  {
								  ID = r.ID,
								  Name = r.Name
							  } );
			return results;
		}

		#endregion ToLookup Method(s)

		#region ToEntityGuidanceMessage Method

		/// <summary>
		/// Converts a <see cref="GuidanceMessage" /> to an <see cref="EntityGuidanceMessage" />.
		/// </summary>
		/// <param name="guidanceMessage"></param>
		/// <returns></returns>
		public static EntityGuidanceMessage ToEntityGuidanceMessage( this GuidanceMessage guidanceMessage )
		{
			return EntityGuidanceMessage.FromGuidanceMessage( guidanceMessage );
		}

		#endregion ToEntityGuidanceMessage Method

		#region ToEntityGuidanceMessages Method

		/// <summary>
		/// Converts a IEnumerable <see cref="GuidanceMessage" /> to an IEnumerable <see
		/// cref="EntityGuidanceMessage" />.
		/// </summary>
		/// <param name="guidanceMessages"></param>
		/// <returns></returns>
		public static IEnumerable<EntityGuidanceMessage> ToEntityGuidanceMessages( this IEnumerable<GuidanceMessage> guidanceMessages )
		{
			return EntityGuidanceMessage.FromGuidanceMessages( guidanceMessages );
		}

		#endregion ToEntityGuidanceMessages Method

		#region ToEntityGuidanceMessage Method

		public static EntityGuidanceMessage ToEntityGuidanceMessage<TModel>( this ErrorInfo errorInfo, GuidanceMessageCode code, GuidanceLevel level, TModel model ) where TModel : IDDModelEntity
		{
			return EntityGuidanceMessage.Create( code, errorInfo.PropertyName, errorInfo.Value, level, model );
		}

		public static EntityGuidanceMessage ToEntityGuidanceMessage( this ErrorInfo errorInfo, GuidanceMessageCode code, GuidanceLevel level, IDDModelEntity entity )
		{
			return EntityGuidanceMessage.Create( code, errorInfo.PropertyName, errorInfo.Value, level, null, entity );
		}

		#endregion ToEntityGuidanceMessage Method

		#region ToEntityGuidanceMessages Method

		public static IEnumerable<EntityGuidanceMessage> ToEntityGuidanceMessages( this IEnumerable<ErrorInfo> errorInfo, GuidanceMessageCode code, GuidanceLevel level = GuidanceLevel.Advisory, IDDModelEntity entity = null )
		{
			var result = from e in errorInfo select EntityGuidanceMessage.Create( code, e.PropertyName, e.Value, level, null, entity );
			return result.ToList();
		}

		#endregion ToEntityGuidanceMessages Method

		#region FromEntityGuidanceMessages Method

		public static IEnumerable<GuidanceMessage> ToGuidanceMessages( this IEnumerable<EntityGuidanceMessage> entityGuidanceMessages, int? transactionID = null, string messagePrefix = "" )
		{
			return from eg in entityGuidanceMessages select eg.ToGuidanceMessage( transactionID, messagePrefix );
		}

		#endregion FromEntityGuidanceMessages Method

		#region SetPriority Methods

		public static void SetPriority( this MailMessage message, int priorityID )
		{
			if ( message == null )
			{
				throw new ArgumentNullException( "message" );
			}
			EmailPriority priority = (EmailPriority)priorityID;
			message.SetPriority( priority );
		}

		public static void SetPriority( this MailMessage message, EmailPriority priority )
		{
			if ( message == null )
			{
				throw new ArgumentNullException( "message" );
			}

			switch ( priority )
			{
				case EmailPriority.High:
					message.Priority = MailPriority.High;
					break;

				case EmailPriority.Normal:
					message.Priority = MailPriority.Normal;
					break;

				case EmailPriority.Low:
					message.Priority = MailPriority.Low;
					break;
			}
		}

		#endregion SetPriority Methods

		#region SetStatus Method

		public static void SetStatus( this Email email, EmailStatus status )
		{
			if ( email == null )
			{
				throw new ArgumentNullException( "email" );
			}
			email.StatusID = (int)status;
		}

		#endregion SetStatus Method

		#region SetStatus Method

		public static void SetStatus( this EDTTransaction transaction, EDTTransactionStatus status )
		{
			transaction.StatusID = (int)status;
		}

		#endregion SetStatus Method

		#region Fill Methods

		public static void Fill<T>( this T viewModel, OrganizationContact orgContact ) where T : ContactRelationship
		{
			if ( orgContact != null )
			{
				viewModel.Context = Context.Organization;
				viewModel.EntityContactID = orgContact.ID;
				viewModel.EntityID = orgContact.OrganizationID;
				viewModel.EntityName = orgContact.Organization.Name;
				viewModel.EntityContactDisplayName = orgContact.Contact.FullName;
				viewModel.Phone = orgContact.Phone;
				viewModel.Title = orgContact.Title;
			}
		}

		public static void Fill<T>( this T viewModel, RegulatorContact regContact ) where T : ContactRelationship
		{
			if ( regContact != null )
			{
				viewModel.Context = Context.Regulator;
				viewModel.EntityContactID = regContact.ID;
				viewModel.EntityID = regContact.RegulatorID;
				viewModel.EntityName = regContact.Regulator.Name;
				viewModel.EntityContactDisplayName = regContact.Contact.FullName;
				viewModel.Phone = regContact.Phone;
				viewModel.Title = regContact.Title;
			}
		}

		#endregion Fill Methods

		#region ToContactRelationship<T> Methods

		public static ContactRelationship ToContactRelationship( this OrganizationContact contact )
		{
			return ToContactRelationship<ContactRelationship>( contact );
		}

		public static ContactRelationship ToContactRelationship( this RegulatorContact contact )
		{
			return ToContactRelationship<ContactRelationship>( contact );
		}

		public static T ToContactRelationship<T>( this OrganizationContact contact ) where T : ContactRelationship, new()
		{
			T result = new T();
			result.Fill( contact );
			return result;
		}

		public static T ToContactRelationship<T>( this RegulatorContact contact ) where T : ContactRelationship, new()
		{
			T result = new T();
			result.Fill( contact );
			return result;
		}

		#endregion ToContactRelationship<T> Methods

		#region ToQuiksoftEmailPriority Method

		public static string ToQuiksoftEmailPriority( this EmailPriority priority )
		{
			string result = "3";
			switch ( priority )
			{
				case EmailPriority.High:
					result = "1";
					break;

				case EmailPriority.Normal:
					result = "3";
					break;

				case EmailPriority.Low:
					result = "4";
					break;
			}
			return result;
		}

		#endregion ToQuiksoftEmailPriority Method
	}
}