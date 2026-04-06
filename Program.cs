using OnlineAlisverisSistemi.Musteriler;
using OnlineAlisverisSistemi.Siparisler;
using OnlineAlisverisSistemi.Urunler;
using System;
using System.ComponentModel;
using System.Security.Cryptography.X509Certificates;
namespace OnlineAlisverisSistemi.Urunler
{
    public class Urun//Ürün classınıı oluşturdum
    {
        public string UrunKodu{ get; private set; }  //tanımlamaları yaptım   
        public string UrunAdi;
        private decimal Fiyat;
        public decimal fiyat//fiyat - girilirse 0 döndürcek
        {
            get { return Fiyat; }
            set
            {
                if (value < 0)
                {
                    Fiyat = 0;
                }
                else
                {
                    Fiyat = value;
                }
                
            }
        }
        public int StokMiktarı{ get; private set; }//properties
        public string Kategori {  get; private set; }
        public int IndirimOrani{ get; set; } =0;
        public static int ToplamUrunSayisi=0;
        public static Urun EnPahaliUrun;
        public static List<Urun> TumUrunler=new List<Urun>();//list tanımı yaptım
        private static Random random = new Random();//random sayı ataması için random nesnesi oluşturdu(static yaptım ki hep farklı sayı versin)
        public const int MinStokSeviyesi = 5;
        public Urun(string urunAdi, decimal _fiyat, int stokMiktari, string kategori)//constructor 
        {
            UrunAdi= urunAdi;
            fiyat = _fiyat;
            StokMiktarı = stokMiktari;
            Kategori = kategori;
            if(kategori=="Elektronik")//Kategoriye göre ürün kodu 
            {
                int sayi = random.Next(1000, 10000);
                UrunKodu = 'E' + sayi.ToString();
            }
            else if(kategori=="Giyim")
            {
                int sayi = random.Next(1000, 10000);
                UrunKodu = "GY" + sayi.ToString();//gıda ile çakışma olmasın diye GY yaptım
            }
            else if(kategori=="Kitap")
            {
                int sayi = random.Next(1000, 10000);
                UrunKodu = 'K' + sayi.ToString();
            }
            else if(kategori=="Gıda")
            {
                int sayi = random.Next(1000, 10000);
                UrunKodu = 'G' + sayi.ToString();
            }
            ToplamUrunSayisi = ToplamUrunSayisi + 1;
            if(EnPahaliUrun==null||EnPahaliUrun.fiyat<_fiyat)
            {
                EnPahaliUrun = this;
            }
            TumUrunler.Add(this);//Listeye bu nesneyi ekliyo
            Console.WriteLine(this.UrunAdi+" Oluşturuldu "+this.UrunKodu);//Ürünün oluşturulduğunu ekrana yazıyor
            Console.WriteLine("----------------------------------------");
            
        }
        public void StokEkle(int miktar)//metotlar var 
        {
            StokMiktarı = StokMiktarı + miktar;
        }
        public bool StokCikar(int miktar)
        {
            int sonuc = StokMiktarı - miktar;
            if(sonuc<0)
            {
                Console.WriteLine("Ürün eklenemez Yetersiz Stok");
                return false;
            }
            else if(sonuc>0&&sonuc<MinStokSeviyesi)
            {
                Console.WriteLine("Uyari:STOK AZALDI");
                StokMiktarı = sonuc;
                return true;
            }
            else
            {
                StokMiktarı = sonuc;
                return true;
            }
        }
        public void IndirimUygula(int yuzde)// - ve 100 den fazla olamıyor  
        {
            if (yuzde < 0) 
            {
                yuzde = 0;
            }
            if (yuzde > 100)
            { 
                yuzde = 100; 
            }
            IndirimOrani = yuzde;
            
        }
        public decimal IndirimliFiyat()//indirim orandı decimal olduğu için decimal tanımlı
        {
            decimal indirildi = Fiyat - (Fiyat * IndirimOrani/100m);
            return indirildi;
        }
        public void BilgiGoster()//ürünün bilgilerini yazıyor
        {
            Console.WriteLine("-----------------------------");
            Console.WriteLine("Ürün Kodu="+UrunKodu);
            Console.WriteLine("Ürün Adı="+UrunAdi);
            Console.WriteLine("Ürün Kategorisi="+Kategori);
            Console.WriteLine("Ürün Fiyatı="+fiyat);
            Console.WriteLine("Ürün Stoğu="+StokMiktarı);
            if (IndirimOrani > 0)//indirimli fiyaatını ekrana yazıyor
            {
                Console.WriteLine($"İndirim  : %{IndirimOrani} (İndirimli: {IndirimliFiyat()} TL)");
            }
            Console.WriteLine("-----------------------------");
        }
        public void StokEkle(int miktar,string tedarikci)//method overloading
        {
            Console.WriteLine(miktar+" adet "+tedarikci+" tedarikçisinden eklendi");
            StokMiktarı = StokMiktarı + miktar;
        }
        public static Urun UrunAra(string urunkodu)
        {
            foreach (var u in TumUrunler)//tüm ürünler listesinden arayıp eğer kodları uyuyorsa geri döndürcek
            {
                if (u.UrunKodu == urunkodu)
                { 
                    return u;
                }  
            }
           return null;
        }
        public static void KategoriyeGoreListele(string kategori)//kategori uyuyosa geri döndürcek
        {
            Console.WriteLine(kategori+" kategorisindeki ürünler listeleniyor=");
            bool bulundu = false;
            foreach(var u in TumUrunler)
            {
                if(u.Kategori==kategori)
                {
                    u.BilgiGoster();
                    bulundu = true;
                }
            }
            if (!bulundu)
            {
                Console.WriteLine("Bu kategoride ürün yok");
            }
        }
        public static void StokDurumuRaporu()
        {
            bool c = false;
            Console.WriteLine("Düşük Stoklu Ürünler!!");
            foreach (var u in TumUrunler)
            {
                if (u.StokMiktarı < MinStokSeviyesi)
                {
                    Console.WriteLine("{0} ({1}) : {2} ",u.UrunAdi,u.UrunKodu,u.StokMiktarı);
                    c = true;
                }
            }
            if(!c)
            {
                Console.WriteLine("Ürünlerin stoğu yeterli");
            }
        }
        public static void EnPahaliUrunuGoster()
        {
            Console.WriteLine("En Pahalı Ürün Getiriliyor");
            Console.WriteLine(EnPahaliUrun.UrunAdi + " " + "("+EnPahaliUrun.fiyat+")");//en pahalı ürünü ve fiyatının getirir
            Console.WriteLine("En pahalı ürünün bilgileri");
            if(EnPahaliUrun!=null)
            {
                EnPahaliUrun.BilgiGoster();//en pahalı ürünün bütün bilgilerini getirir
            }
            else
            {
                Console.WriteLine("Henüz ürün yüklenmedi");
            }
        }
    }
}


namespace OnlineAlisverisSistemi.Musteriler

{
    public class Musteri//Müşteri classını oluşturdum
    {
        public string MusteriNo {  get; private set; }//tanımlamaları yaptım
        public string AdSoyad;
        public string Email;
        public string Telefon;
        public string Adres;
        private static int sayac = 1001;
        public string UyelikTarihi {  get; private set; }
        public decimal ToplamHarcama {  get; private set; }
        public static int ToplamMusteriSayisi;
        public static int SonMusteriNo;
        public static List<Musteri> TumMusteriler = new List<Musteri>();
        public Musteri(string adSoyad, string email, string telefon, string adres)//constructor 
        {
            AdSoyad = adSoyad;
            Email = email;
            Telefon = telefon;
            Adres = adres;
            MusteriNo = 'M' + sayac.ToString();
            SonMusteriNo = sayac;
            sayac = sayac + 1;
            UyelikTarihi = DateTime.Now.Year.ToString();
            ToplamMusteriSayisi = ToplamMusteriSayisi + 1;
            TumMusteriler.Add(this);
            Console.WriteLine( this.AdSoyad+" kaydedildi {0}",this.MusteriNo);
            Console.WriteLine("---------------------------------------------");
        }
        public void HarcamaEkle(decimal tutar)//metotlar
        {
            ToplamHarcama = ToplamHarcama + tutar;
        }
        public string MusteriTipi()//string döndürüyor
        {
            if(ToplamHarcama>=10000)
            {
                
                return "VIP Müşteri";
            }
            else if(ToplamHarcama>=5000)
            {
                
                return "GOLD MÜŞTERİ";
            }
            else if(ToplamHarcama>=1000)
            {
               
                return "Silver Müşteri";
            }
            else
            {
                
                return "Standart Müşteri";
            }
        }
        public void BilgiGoster()//müşteri bilgileri
        {
            Console.WriteLine("-------------------------------------------");
            Console.WriteLine("Müşteri No="+MusteriNo);
            Console.WriteLine("Müşteri Ad Soyad="+AdSoyad);
            Console.WriteLine("Müşteri Emaili="+Email);
            Console.WriteLine("Müşteri Telefonu"+Telefon);
            Console.WriteLine("Müşteri Adresi="+Adres);
            Console.WriteLine("Müşteri Üyelik Tarihi="+UyelikTarihi);
            Console.WriteLine("Müşteri Tipi= " + MusteriTipi());
            Console.WriteLine("-------------------------------------------");
        }
        public static Musteri MusteriAra(string musteriNo)
        {
            foreach(var m in TumMusteriler)//listeden müşteri noya göre müşteri arıyor
            {
                if(m.MusteriNo==musteriNo)
                {
                    return m;
                }
            }
            return null;
        }
        public static Musteri VIPMusterileriListele()//Listeden Vip müşterileri getiriyor
        {
            Console.WriteLine("VİP müşteriler listeleniyor");
            foreach(var m in TumMusteriler)
            {
                if(m.MusteriTipi() == "VIP Müşteri")
                {
                    m.BilgiGoster();
                }
            }
            return null;
        }
        public static void ToplamHarcamaRaporu()//tüm müşterilerin harcama raporu
        {
            foreach( var m in TumMusteriler)
            {
                Console.WriteLine(m.MusteriNo+" No lu Müşterinin Toplam Harcaması "+m.ToplamHarcama);
            }
            
        }
        
    }
}



namespace OnlineAlisverisSistemi.Siparisler
{
    using OnlineAlisverisSistemi.Musteriler;//yukarıdaki namespaceleri kullanacağım için using ile kullanacağımı söyledim
    using OnlineAlisverisSistemi.Urunler;
using System.Runtime.InteropServices;

    public class SepetUrunu//sepet urunu classı
    {
        public string UrunKodu;//tanımlamalar
        public string UrunAdi;
        public decimal BirimFiyat;
        public int Adet;
        public decimal ToplamFiyat{  get; private set; }
        public SepetUrunu(string urunKodu, string urunAdi, decimal birimFiyat, int adet)//constructor
        {
            UrunKodu=urunKodu;
            UrunAdi=urunAdi;
            BirimFiyat=birimFiyat;
            Adet = adet;
            ToplamFiyat = BirimFiyat * adet;
        }
        public void AdetArttir(int miktar)//metotlar
        {
            Adet = Adet + miktar;
            ToplamFiyat = ToplamFiyat+(miktar * BirimFiyat);//toplam fiyatı adet arttıkça güncelleniyor
        }
        public void AdetAzalt(int miktar)
        {
            if (miktar > Adet)
            {
                Adet = 0;
            }
            else
            {
                Adet = Adet - miktar;
                ToplamFiyat=ToplamFiyat-(miktar * BirimFiyat);//toplam fiyat güncelleniyor
            }
        }
        public void BilgiGoster()//ürünün bilgilerini getiriyor
        {
            Console.WriteLine("---------------------------------------");
            Console.WriteLine("Ürün Kodu= "+UrunKodu);
            Console.WriteLine("Ürün Adı= "+UrunAdi);
            Console.WriteLine("Ürün Birim Fiyatı= "+BirimFiyat);
            Console.WriteLine("Ürün Adedi= "+Adet);
            Console.WriteLine("Ürün Toplam Fiyatı= "+ToplamFiyat);
            Console.WriteLine("----------------------------------------");
        }
    }
    public class Siparis//siparis classı
    {
        public string SiparisNo {  get; private set; }//tanımlamlar
        public string MusteriNo;
        public List<SepetUrunu> SepetUrunleri = new List<SepetUrunu>();//sepet ürünleri listesi sepetteki ürünleri tutcak
        public decimal ToplamTutar {  get; private set; }//properties
        public string SiparisDurumu {  get; private set; }
        public string SiparisTarihi {  get; private set; }
        public static int ToplamSiparisSayisi;
        public static int sayac2 = 5001;
        public static int SonSiparisNo;
        public static decimal ToplamCiro;
        public static List<Siparis> TumSiparisler=new List<Siparis>();
        public const decimal KargoUcreti = 50;//sabitler
        public const decimal UcretsizKargoLimiti = 500;
        public Siparis(string musteriNo)//constructor
        {
            MusteriNo = musteriNo;
            SiparisNo = "SIP" + sayac2.ToString();
            SonSiparisNo = sayac2;
            sayac2 = sayac2 + 1;
            SepetUrunleri = new List<SepetUrunu>();
            SiparisDurumu = "Hazırlanıyor";
            SiparisTarihi = DateTime.Now.Year.ToString();
            ToplamSiparisSayisi = ToplamSiparisSayisi + 1;
            TumSiparisler.Add(this);
        }
        public void UrunEkle(string urunKodu,int adet)// ürün ekle metodu sepete ürün ekliyor
        {
            Urun urun=Urun.UrunAra(urunKodu);//ürün koduna göre ürünü buldu
            bool stokkontrol = urun.StokCikar(adet);//sepete ekleyeceği adet kadar stokta var mı diye baktım
            if(stokkontrol==false)
            {
                Console.WriteLine("Ürün Sepete eklenemedi");
            }
            
            else if (stokkontrol==true)//eğer varsa
            {
                decimal indfiyat = urun.IndirimliFiyat();
                SepetUrunu yeniurun = new SepetUrunu(urun.UrunKodu, urun.UrunAdi, indfiyat, adet);//sepet için yeniurun oluşturdum
                Console.WriteLine("Sepete ekleniyor");
                SepetUrunleri.Add(yeniurun);//sepetürünleri listesine ekledim yeniürünü
                ToplamTutar =ToplamTutar+yeniurun.ToplamFiyat;//toplam tutarı güncelledim
                
                Console.WriteLine(yeniurun.UrunAdi+" "+" x "+yeniurun.Adet+" - "+yeniurun.BirimFiyat);//sepete eklenen ürünleri gösterir
                Console.WriteLine("---------------------------------------------------------------");
            }
        }
        public void UrunCikar(string urunKodu)
        {
            SepetUrunu silinecek = null;
            foreach(var i in SepetUrunleri)//ürünkodunda silinecek olanı buldum
            {
                if(i.UrunKodu==urunKodu)
                {
                    silinecek = i;
                    break;//döngüden çıktım
                }
            }
            if(silinecek!=null)
            {
                Urun BastakiUrun = Urun.UrunAra(urunKodu);//ana ürünü buldum sepettekinin harici olanları
                BastakiUrun.StokEkle(silinecek.Adet);//silinecek olanın adetini ana ürünün stoğuna ekledim
                ToplamTutar = ToplamTutar - (silinecek.Adet * silinecek.BirimFiyat);//fiyatı güncelledim
                SepetUrunleri.Remove(silinecek);//silinecek olan sepetten sildim
                Console.WriteLine("{0} Sepetten çıkarılıyor Stoğu geri yüklendi", silinecek.UrunAdi);
            }
            else
            {
                Console.WriteLine("Çıkarmak İstediğiniz Ürün Sepette Yok");
            }
         
        }
        public void SepetiBosalt()
        {
            foreach(var i in SepetUrunleri)
            {
                Urun BastakiUrun = Urun.UrunAra(i.UrunKodu);//yine bastaki ürünü buldum
                BastakiUrun.StokEkle(i.Adet);//hepsi silinecek o yüzden arama yapmama gerek yok hepsinin stoğunu geri ekledim
            }
            SepetUrunleri.Clear();//sepet ürünlerini sildim hiçbir şey yok artık
            ToplamTutar = 0;//tutarı güncelledim
            Console.WriteLine("-------Sepet Boşaltıldı--------");
        }
        public decimal KargoUcretiHesapla()
        {
            if(ToplamTutar>=UcretsizKargoLimiti)//limit 500 eğer geçerse toplam tutar kargo ücreti 0 TL
            {
                return 0;
            }
            else
            {
                return KargoUcreti;//geçmezse kargo ücreti döncek
            }
        }
        public decimal GenelToplamHesapla()
        {
            decimal kargoo = KargoUcretiHesapla();
            return ToplamTutar+kargoo;//en son bütün herşeyi hesaplar
        }
        public void SiparisiTamamla()//sipariş tamamlanıyor
        {
            decimal odenecektutar=GenelToplamHesapla();
            Musteri musteri = Musteri.MusteriAra(MusteriNo);
            if(musteri!=null)
            {
                musteri.HarcamaEkle(odenecektutar);
            }
            ToplamCiro = ToplamCiro + odenecektutar;
            SiparisDurumu = "Kargoda";//sipariş durumunu güncelledim
            Console.WriteLine("-------------------");
            Console.WriteLine("Sipariş Tamamlandı");
            Console.WriteLine("-------------------");
        }
        public void DurumGuncelle(string yeniDurum)//sipariş durumunu günceller
        {
            SiparisDurumu = yeniDurum;
        }
        public void SepetGoster()//sepet içeriği
        {
            Console.WriteLine("Sepetteki ürünler listeleniyor");
            foreach(var item in SepetUrunleri)
            {
                item.BilgiGoster();//sepetteki ürünlerin detaylı bilgilerini gösterir
            }
            decimal kargoo = KargoUcretiHesapla();//kargoo metottan geldi
            Console.WriteLine("Ara Tutar= "+ToplamTutar);//ara tutar
            if (kargoo == 0)
            {
                Console.WriteLine("500 TL üzeri olduğu için");
                Console.WriteLine("Kargo Ücreti= " + kargoo + " TL");
            }
            else
            {
                Console.WriteLine("Kargo Ücreti= " + kargoo + " TL");
            }
            Console.WriteLine("Genel Toplam= "+(kargoo+ToplamTutar)+" TL");//genel toplam hesaplandı
        }
        public void FaturaYazdir()//fatura
        {
            Musteri musteri = Musteri.MusteriAra(MusteriNo);
            Console.WriteLine("--------Sipariş Faturası----------");
            Console.WriteLine("Sipariş No = "+this.SiparisNo);//sipariş no sunu yazdı
            Console.WriteLine("Müşteri Bilgileri " + musteri.AdSoyad + " " + musteri.MusteriNo);//müşteri ad soyadı yazdı
            Console.WriteLine("Sipariş Tarihi " + this.SiparisTarihi);//sipariş tarihini yazdı
            decimal kargoo = KargoUcretiHesapla();//kargo ücretini buldu
            Console.WriteLine("Ara Tutar= " + ToplamTutar);//ara tutarı yazdı
            if (kargoo == 0)
            {
                Console.WriteLine("500 TL üzeri olduğu için");
                Console.WriteLine("Kargo Ücreti= " + kargoo + " TL");//kargo ücreti yazdı
            }
            else
            {
                Console.WriteLine("Kargo Ücreti= " + kargoo + " TL");
            }
            Console.WriteLine("Genel Toplam= " + (ToplamTutar+kargoo));//en son genel toplamı hesaplayıp sona yazdı
            
        }
        public static void SiparisAra(string siparisNo)
        {
            bool bulundu = false;
            foreach(var i in TumSiparisler)
            {
                if(i.SiparisNo==siparisNo)//tüm siparişler listesinden sipariş arama
                {
                    Console.WriteLine(i.SiparisNo+" No lu siparişiniz için");
                    Console.WriteLine("Sipariş Tarihi= "+i.SiparisTarihi);
                    Console.WriteLine("Sipariş Durumu= "+i.SiparisDurumu);
                    bulundu = true;
                }
            }
            if(bulundu==false)
            {
                Console.WriteLine("Sipariş Bulunamadı");
            }
            
        }
        public static void MusteriyeGoreSiparisler(string musteriNo)//müşteriye göre sipariş arama
        {
            Musteri musteri = Musteri.MusteriAra(musteriNo);
            Console.WriteLine(musteriNo+" Nolu Müşterinin Siparişleri");
            foreach(var i in TumSiparisler)
            {
                if(i.MusteriNo==musteri.MusteriNo)//eğer müşterinin varsa siparişi
                {
                    Console.WriteLine("------------------------------------------------");
                    Console.WriteLine(i.SiparisNo + "No lu siparişiniz için");//sipariş no yazar
                    i.SepetGoster();
                    Console.WriteLine("Sipariş Tarihi= " + i.SiparisTarihi);//tarihini yazar
                    Console.WriteLine("Sipariş Durumu= " + i.SiparisDurumu);//durumunu yazar
                    Console.WriteLine("------------------------------------------------");
                }

            }

        }
        public static void GenelRapor()//en son bütün herşeyin raporunu verir
        {
            Console.WriteLine("-------------------------------------------------");
            Console.WriteLine("Toplam Ürün Sayısı = "+Urun.ToplamUrunSayisi);//toplam ürün sayısını yazar(static olduğu için urun classından çağırdım)
            Console.WriteLine("Toplam Müşteri Sayısı = "+Musteri.ToplamMusteriSayisi);//toplam müşteri sayısını yazar(Static olduğu için sınıfdan çağırılır)
            Console.WriteLine("Toplam Sipariş Sayısı= "+ToplamSiparisSayisi);//toplam sipariş sayısı
            Console.WriteLine("Toplam Ciro= "+ToplamCiro+"TL");//toplam ciro
            Console.WriteLine("-------------------------------------------------");
        }
    }


}
namespace OnlineAlisverisSistemi
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("---------------ÜRÜN TANIMLAMA-----------------------");
            Urun u1 = new Urun("Laptop", 25000, 10, "Elektronik");
            Urun u2 = new Urun("Akıllı Telefon", 35000, 20, "Elektronik");
            Urun u3 = new Urun("Kışlık Mont", 2500, 50, "Giyim");
            Urun u4 = new Urun("Kot Pantolon", 800, 4, "Giyim");
            Urun u5 = new Urun("Sefiller", 350, 100, "Kitap");
            Urun u6 = new Urun("Ezine Peyniri", 450, 30, "Gıda");
            Urun u7 = new Urun("Obruk Peyniri", 200, 0, "Gıda");
            Urun u8 = new Urun("Pil", -100, 15, "Elektronik");
            Urun u9 = new Urun("Mouse", 500, 3, "Elektronik");
            Urun u10 = new Urun("Klavye", 400, 2, "Elektronik");
            u3.IndirimUygula(20);
            u5.IndirimUygula(10);
            Console.WriteLine("-------------ÜRÜNLERİN BİLGİLERİ------------------");
            u1.BilgiGoster();
            u2.BilgiGoster();
            u3.BilgiGoster();
            u4.BilgiGoster();
            u5.BilgiGoster();
            u6.BilgiGoster();
            Console.WriteLine("");
            Urun.EnPahaliUrunuGoster();
            Console.WriteLine("---------------------");
            Console.WriteLine("----------------MÜŞTERİLER OLUŞTU----------------");
            Musteri m1 = new Musteri("Hüseyin Şentürk", "huse@gmail.com", "0555 111 22 33", "İstanbul/Kadıköy");
            Musteri m2 = new Musteri("Berke Demir", "brk@hotmail.com", "0532 444 55 66", "Ankara/Çankaya");
            Musteri m3 = new Musteri("Uğur Taş", "ugrt@yahoo.com", "0505 777 88 99", "İzmir/Karşıyaka");
            Console.WriteLine("------------MÜŞTERİ BİLGİLERİ-----------------");
            m1.BilgiGoster();
            m2.BilgiGoster();
            m3.BilgiGoster();
            Console.WriteLine("-----------------------------");//1. sipariş
            Console.WriteLine("------------SİPARİŞ OLUŞTURMA-----------------------");
            Siparis s1 = new Siparis(m1.MusteriNo);
            s1.UrunEkle(u1.UrunKodu, 1);
            s1.UrunEkle(u3.UrunKodu, 2);
            s1.UrunEkle(u6.UrunKodu, 3);
            s1.UrunEkle(u4.UrunKodu, 2);
            s1.SepetGoster();
            s1.GenelToplamHesapla();
            s1.SiparisiTamamla();
            s1.FaturaYazdir();
            Console.WriteLine("------------------------------");//2.sipariş
            Console.WriteLine("------------2 SİPARİŞ OLUŞTURMA-----------------");
            Siparis s2=new Siparis(m2.MusteriNo);
            s2.UrunEkle(u2.UrunKodu, 5);
            s2.UrunEkle(u5.UrunKodu, 50);
            s2.UrunCikar(u2.UrunKodu);
            s2.SiparisiTamamla();
            Console.WriteLine("-------------------------------");//Stok Kontrolü
            Console.WriteLine("-----------STOK KONTROLÜ----------------");
            Urun.StokDurumuRaporu();
            u2.StokEkle(10, "VATAN BİLGİSAYAR");
            Console.WriteLine("-------------------------------");//Müşteri Raporları
            Console.WriteLine("----------------MÜŞTERİ RAPORLARI-------------------");
            Console.WriteLine("1 . Müşterinin Durumu= "+m1.MusteriTipi());
            Musteri.VIPMusterileriListele();
            Console.WriteLine("---Harcama Raporları---");
            Musteri.ToplamHarcamaRaporu();
            Console.WriteLine("--------------------------------------------");//Genel Raporlar
            Console.WriteLine("-----------GENEL RAPORLAR--------------------");
            Urun.KategoriyeGoreListele("Elektronik");
            Console.WriteLine("---Müşteriye Göre Siparişler---");
            Siparis.MusteriyeGoreSiparisler(m1.MusteriNo);
            Console.WriteLine("---Genel Raporlar---");
            Siparis.GenelRapor();
            Console.WriteLine("-----------------------------------------------");//Özel Durumlar    
            Console.WriteLine("-------------------ÖZEL DURUMLAR---------------");
            Siparis s4 = new Siparis(m3.MusteriNo);
            s4.UrunEkle(u7.UrunKodu, 5);//Stok yok mesajı basıyor
            u8.BilgiGoster();//0 atadı
            u2.IndirimUygula(120);
            u2.BilgiGoster();//120 girmeme rağmen 100 olarak atadı 





            




        }
    }
}

    


