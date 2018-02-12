using CERS.SystemServices;
using UPF.Core;

namespace CERS
{
	public interface ICERSSystemServiceManager
	{
		BusinessService BusinessLogic { get; }

		ICoreSystemServiceManager CoreServices { get; }

        DeferredJobService DeferredJob { get; }

		EmailService Emails { get; }

		ErrorReportingService ErrorReporting { get; }

		EventService Events { get; }

		ExcelService Excel { get; }

		FacilityService Facilities { get; }

		FacilitySubmittalModelEntityService FacilitySubmittalModelEntities { get; }

		FacilitySubmittalService FacilitySubmittals { get; }

		GeoService Geo { get; }

		NotificationService Notification { get; }

		PdfService PDF { get; }

        PrintJobService PrintJob { get; }

        JobDocumentCleanUpService JobDocumentCleanUp { get; }

		ReportService Reports { get; }

		ICERSRepositoryManager Repository { get; }

		SecurityService Security { get; }

		SubmittalDeltaService SubmittalDelta { get; }

		TextMacroProcessingService TextMacroProcessing { get; }

		ViewModelService ViewModels { get; }

		XmlService Xml { get; }

		TService GetService<TService>() where TService : class, ICERSSystemService;
	}
}