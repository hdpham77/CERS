using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CERS.Model;
using UPF.ViewModel;

namespace CERS
{
	public interface IFacilitySubmittalElementResourceViewModel<TModel> : IViewModel<TModel> where TModel : class, IFacilitySubmittalModelEntity
	{
		//TModel Entity
		//{
		//    get;
		//    set;
		//}

		int? OrganizationID
		{
			get;
		}

		int? CERSID
		{
			get;
		}

		int? FSEID
		{
			get;
		}

		int? FSERID
		{
			get;
		}

		SubmittalElementType? SubmittalElement
		{
			get;
		}

		FacilitySubmittalElement FacilitySubmittalElement { get; set; }

		bool EditingAllowed { get; set; }

		FacilitySubmittalElementResource FacilitySubmittalElementResource { get; set; }
	}
}