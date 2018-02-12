using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CERS
{
	public interface IRepositoryForEntityWithEDTIdentityKey<TModel> where TModel : IModelEntityWithEDTIdentityKey
	{
		TModel GetByEDTIdentityKey(int edtIdentityKey);
	}
}