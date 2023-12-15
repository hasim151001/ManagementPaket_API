namespace ManagementPaket_API.Model
{
    public class TrsPengambilanModel
    {
        public int pak_id_transaksi { get; set; }
        public int pak_id { get; set; }
        public string pak_id_pengambil { get; set; }
        public string pak_nama_pengambil { get; set; }
        public DateTime? pak_tanggal_pengambilan { get; set; }
        public string pak_foto_pengambil { get; set; }
        public int pak_status_transaksi { get; set; }
        public string pak_nama_pemilik { get; set; }
        public string pak_nama_pengirim { get; set; }
    }
}
