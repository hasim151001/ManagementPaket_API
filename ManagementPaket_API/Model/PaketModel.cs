namespace ManagementPaket_API.Model
{
    public class PaketModel
    {
        public string pak_id { get; set; }
        public string pak_nama_pemilik { get; set; }
        public string pak_id_jenis { get; set; }
        public DateTime pak_tanggal_sampai { get; set; }
        public string pak_nama_pengirim { get; set; }
        public string pak_status { get; set; }
    }

}
