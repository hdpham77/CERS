using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UPF;

namespace CERS
{
	public interface ICERSCrudableRepository<TModel> : ICERSRepository where TModel : IModelEntityWithID, new()
	{
		void Create( TModel entity );

		void Delete( TModel entity );

		void Detach( TModel entity );

		void Save( TModel entity );

		void Update( TModel entity );
	}
}