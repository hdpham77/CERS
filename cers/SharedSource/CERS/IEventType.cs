using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UPF;

namespace CERS
{
    public interface IEventType : IModelEntityWithID
    {
        int DefaultDeliveryMechanismID { get; set; }

        int DefaultOrganizationPriorityID { get; set; }

        int DefaultRegulatorPriorityID { get; set; }

        int? DefaultEmailTemplateID { get; set; }

        int? DefaultNotificationTemplateID { get; set; }

        int DefaultExpirationWindowInDays { get; set; }

        string Description { get; set; }

        bool Enabled { get; set; }

        bool Expires { get; set; }

        string Name { get; set; }
    }
}