using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MyLab.EmailManager.Infrastructure.Db.EfModels;

[Table("email")]
public partial class DbEmail
{
    [Key]
    [Column("id")]
    [MySqlCharSet("ascii")]
    [MySqlCollation("ascii_general_ci")]
    public Guid Id { get; set; }

    [Column("deleted")]
    public bool Deleted { get; set; }

    [Column("deleted_dt")]
    [MaxLength(6)]
    public DateTime? DeletedDt { get; set; }

    [Column("address")]
    public string Address { get; set; } = null!;

    public virtual DbConfirmation? Confirmation { get; set; }

    [InverseProperty("Email")]
    public virtual ICollection<DbLabel> Labels { get; set; } = new List<DbLabel>();

    [InverseProperty("Email")]
    public virtual ICollection<DbMessage> Messages { get; set; } = new List<DbMessage>();
}
