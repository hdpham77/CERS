using CERS.Compositions;
using CERS.Model;
using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Xml.Linq;
using UPF;

namespace CERS
{
	public static class ExtensionMethods
	{
		/// <summary>
		/// Perform a deep Copy of the object.
		/// </summary>
		/// <typeparam name="T">The type of object being copied.</typeparam>
		/// <param name="source">The object instance to copy.</param>
		/// <returns>The copied object.</returns>
		public static T Clone<T>( this T source )
		{
			if ( !typeof( T ).IsSerializable )
			{
				throw new ArgumentException( "The type must be serializable.", "source" );
			}

			// Don't serialize a null object, simply return the default for that object
			if ( Object.ReferenceEquals( source, null ) )
			{
				return default( T );
			}

			IFormatter formatter = new BinaryFormatter();
			Stream stream = new MemoryStream();
			using ( stream )
			{
				formatter.Serialize( stream, source );
				stream.Seek( 0, SeekOrigin.Begin );
				return (T)formatter.Deserialize( stream );
			}
		}

		public static string GetPropertyPath<TModel, TProperty>( Expression<Func<TModel, TProperty>> selector )
		{
			StringBuilder sb = new StringBuilder();
			MemberExpression memberExpr = selector.Body as MemberExpression;
			while ( memberExpr != null )
			{
				string name = memberExpr.Member.Name;
				if ( sb.Length > 0 )
					name = name + ".";
				sb.Insert( 0, name );
				if ( memberExpr.Expression is ParameterExpression )
					return sb.ToString();
				memberExpr = memberExpr.Expression as MemberExpression;
			}
			throw new ArgumentException( "The expression must be a MemberExpression", "selector" );
		}

		public static IEnumerable<ISystemLookupEntity> GetValues( this LookupTableCacheItemCollection<ISystemLookupEntity> collection, SystemLookupTable lookupTable )
		{
			List<ISystemLookupEntity> results = new List<ISystemLookupEntity>();
			var cachedItem = collection.SingleOrDefault( p => p.Name == lookupTable.ToString() );
			if ( cachedItem != null )
			{
				results.AddRange( cachedItem.Values );
			}
			return results;
		}

		public static bool HasErrors( this FacilityCreatePackage package )
		{
			if ( package == null )
			{
				return false;
			}
			return package.Workspace.Messages.HasErrors();
		}

		public static bool HasErrors( this IEnumerable<FacilityOperationMessage> messages )
		{
			if ( messages == null )
			{
				return false;
			}
			return ( messages.Count( p => p.Type == FacilityOperationMessageType.Error ) > 0 );
		}

		public static ObjectQuery<TModel> Include<TModel, TProperty>( this ObjectQuery<TModel> objectQuery, Expression<Func<TModel, TProperty>> selector )
		{
			string propertyPath = GetPropertyPath( selector );
			return objectQuery.Include( propertyPath );
		}

		public static int? RegulatorIDBySubmittalElement( this IEnumerable<RegulatorZipCodeSubmittalElement> rzseMappings, SubmittalElementType submittalElement )
		{
			int? result = null;
			if ( rzseMappings != null )
			{
				var mapping = rzseMappings.SingleOrDefault( p => p.SubmittalElementID == (int)submittalElement && !p.Voided );
				if ( mapping != null )
				{
					result = mapping.RegulatorID;
				}
			}
			return result;
		}

		public static List<int> ToIDs( this IEnumerable<IEntityWithID> entities )
		{
			var results = from e in entities select e.ID;
			return results.ToList();
		}

		public static IEnumerable<SystemLookupEntity> ToLookupEntity( this IEnumerable<ISystemLookupEntity> entities )
		{
			var results = from e in entities
						  select new SystemLookupEntity
						  {
							  ID = e.ID,
							  Name = e.Name,
							  Description = e.Description,
							  CreatedByID = e.CreatedByID,
							  CreatedOn = e.CreatedOn,
							  UpdatedByID = e.UpdatedByID,
							  UpdatedOn = e.UpdatedOn,
							  Voided = e.Voided
						  };

			return results;
		}

		public static SystemLookupEntity ToLookupEntity( this ISystemLookupEntity entity )
		{
			SystemLookupEntity results = new SystemLookupEntity
			{
				ID = entity.ID,
				Name = entity.Name,
				Description = entity.Description,
				CreatedByID = entity.CreatedByID,
				CreatedOn = entity.CreatedOn,
				UpdatedByID = entity.UpdatedByID,
				UpdatedOn = entity.UpdatedOn,
				Voided = entity.Voided
			};
			return results;
		}
	}
}