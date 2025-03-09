using System.ComponentModel.DataAnnotations.Schema;

namespace FirstApi.Models
{
    [Table("t_activetokens")]
    public class ActiveToken
    {
        [Column("c_activetokenid")]
        public int ActiveTokenId { get; set; }= 0;

        [Column("c_userid")]
        public int UserId { get; set; }

        [Column("c_usertoken")]
        public string UserToken { get; set; }

        [Column("c_tokenissuedatutc")]
        public DateTime TokenIssuedAtUtc { get; set; }

        [Column("c_tokenexpiryatutc")]
        public DateTime TokenExpiryAtUtc { get; set; }
     
    }
}
