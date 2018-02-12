using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace CERS
{
    public class DataElementCodeCollection :Collection<IDataElementCode>
    {
        public DataElementCodeCollection( IEnumerable<IDataElementCode> codes )
        {
            AddRange( codes );
        }

        public DataElementCodeCollection()
        {
        }

        public void AddRange( IEnumerable<IDataElementCode> codes )
        {
            foreach ( var code in codes )
            {
                Add( code );
            }
        }

        public bool ContainsCode( string targetCode )
        {
            return this.Count( p => p.Code == targetCode ) > 0;
        }

        public void RemoveCode( string targetCode )
        {
            var code = this.SingleOrDefault( p => p.Code == targetCode );
            if ( code != null )
            {
                this.Remove( code );
            }
        }

        public void RemoveObsoleteCodes( string targetCode = null )
        {
            //obtain all obsolete codes
            var obsoleteCodes = this.Where( p => p.Obsolete ).Select( p => p.Code ).ToList();

            //loop through all obsolete codes
            foreach ( var obsoleteCode in obsoleteCodes )
            {
                //if a targetcode was specified make sure we don't remove
                //if it's in the obsolete list.
                if ( !string.IsNullOrEmpty( targetCode ) )
                {
                    //if the obsoleteCode is not equal to the targetCode then remove it.
                    if ( obsoleteCode != targetCode )
                    {
                        RemoveCode( obsoleteCode );
                    }
                }
                else
                {
                    RemoveCode( obsoleteCode );
                }
            }
        }
    }
}