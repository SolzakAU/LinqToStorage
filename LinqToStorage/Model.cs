using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqToStorage
{
  class ContactInstance
  {
    internal int Key { get; set; }

    public virtual ICollection<ContactAddress> Address { get; set; }
    public virtual ICollection<ContactPhone> Phone { get; set; }
    public virtual ICollection<ContactEmail> Email { get; set; }
  }

  class ContactAddress
  {
    internal int Key { get; set; }

    public string Location { get; set; }
  }

  class ContactPhone
  {
    internal int Key { get; set; }

    public string Number { get; set; }
    public string Description { get; set; }
    public bool Preferred { get; set; }
  }

  class ContactEmail
  {
    internal int Key { get; set; }

    public string Address { get; set; }
  }

  class PatientName
  {
    internal int Key { get; set; }

    public string Title { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
  }

  class PatientRecord
  {
    internal int Key { get; set; }

    public PatientName PreferredName { get; set; }
    public virtual ICollection<PatientName> Name { get; set; }
    public virtual ICollection<PatientIdentifier> Identifier { get; set; }
    public virtual ICollection<PatientNote> Note { get; set; }
  }

  class PatientIdentifier
  {
    internal int Key { get; set; }
    internal int PatientRecordKey { get; set; }

    public PatientIdentifierType PatientIdentifierType { get; set; }
    public string Value { get; set; }
    public bool Deleted { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public bool Preferred { get; set; }
  }

  class PatientIdentifierType
  {
    internal int Key { get; set; }

    public string Code { get; set; }
    public string Name { get; set; }
    public bool Shared { get; set; }
  }

  class PatientConditionInstance
  {
    internal int Key { get; set; }

    public DateTimeOffset? StartDateTime;
    public DateTimeOffset? FinishDateTime;
    public PatientConditionDefinition PatientConditionDefinition;
  }

  class PatientConditionDefinition
  {
    internal int Key { get; set; }

    public string Code { get; set; }
    public string Name { get; set; }
  }

  class PatientNote
  {
    internal int Key { get; set; }

    public int NoteStyle { get; set; }
    public bool Deleted { get; set; }
    public byte[] Buffer { get; set; }
  }

  class RequestRecord
  {
    internal int Key { get; set; }

    public PatientRecord Patient { get; set; }
    public DateTimeOffset? RegisteredDate { get; set; }
    public RequestService[] Service { get; set; }
  }

  class RequestService
  {
    internal int Key { get; set; }

    public int Sequence { get; set; }
    public ServiceDefinition Ordered { get; set; }
    public ServiceDefinition Performed { get; set; }
  }

  class RequestServiceStep
  {
    internal int Key { get; set; }

    public int Sequence { get; set; }
    public int Status { get; set; }
  }

  class ServiceDefinition
  {
    internal int Key { get; set; }

    public string Code { get; set; }
    public string Name { get; set; }
  }

  class ReportInstance
  {
    internal int Key { get; set; }

    public RequestRecord Request { get; set; }
    public RequestService[] Service { get; set; }
  }
}
