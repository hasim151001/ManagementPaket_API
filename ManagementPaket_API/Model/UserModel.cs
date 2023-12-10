namespace ManagementPaket_API.Model
{
    public class UserModel
    {
        public int pak_id_user { get; set; }
        public string pak_nama_user { get; set; }
        public string pak_username { get; set; }
        public string pak_password { get; set; }
        public byte[] pak_foto { get; set; } // Gunakan byte[] untuk menyimpan data gambar
        public string pak_role { get; set; }
    }

}
