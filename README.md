# 🖋️ XSLT Editor 

<img width="1630" height="1036" alt="abc" src="https://github.com/user-attachments/assets/a390e4bf-9fd0-4bb2-9ad7-35754a064052" />


![License](https://img.shields.io/github/license/dogukankosan/XSLTEditor)
![Stars](https://img.shields.io/github/stars/dogukankosan/XSLTEditor)
![Issues](https://img.shields.io/github/issues/dogukankosan/XSLTEditor)
![Last Commit](https://img.shields.io/github/last-commit/dogukankosan/XSLTEditor)

> **XSLT Editor**, e-Fatura ve e-İrsaliye belgelerinin XSLT şablonlarını tasarlamak, düzenlemek ve anlık önizlemek için geliştirilmiş profesyonel bir masaüstü C#/.NET uygulamasıdır.

---

## 🚀 Özellikler

- 📄 **XSLT 1.0 ve 2.0/3.0 desteği** — Native .NET ve Saxon-HE motoru ile otomatik algılama
- 🔍 **Anlık XML önizleme** — CefSharp tabanlı yerleşik tarayıcı ile gerçek zamanlı HTML çıktısı
- ✏️ **Gelişmiş kod editörü** — ICSharpCode TextEditor ile sözdizimi vurgulama, satır numaraları, kod katlama
- ⚡ **Otomatik tamamlama** — XSLT ve UBL namespace tag'leri için Ctrl+Space ile akıllı öneri
- ⚠️ **Gerçek zamanlı sözdizimi kontrolü** — Hata satırları kırmızı dalgalı çizgi ve gutter ikonu ile işaretlenir
- 🔎 **Metin arama** — Ctrl+F ile büyük/küçük harf duyarlı arama
- 📂 **Belge türü otomatik algılama** — XSLT ve XML içeriğine göre e-Fatura / e-İrsaliye otomatik seçilir
- 🔒 **Donanım tabanlı lisanslama** — SHA256 Hardware ID ile sunucu üzerinden lisans doğrulama
- 🗝️ **Güvenli kimlik bilgisi yönetimi** — AES-256-GCM şifreli SQLite tabanlı token & ayar saklama
- 📝 **Günlük log sistemi** — Tüm işlemler tarih bazlı LOG klasörüne kaydedilir
- 🎨 **DevExpress Ribbon arayüzü** — Modern ve kullanıcı dostu görünüm

---

## 🗂 Proje Yapısı

```
XSLTEditor/
├── Classes/
│   ├── AppConfig.cs               # app.config okuma (API URL)
│   ├── CredentialManager.cs       # AES-GCM şifreli SQLite ile kimlik & token yönetimi
│   ├── HardwareInfo.cs            # WMI ile donanım ID üretimi (SHA256)
│   ├── InvoiceLoader.cs           # XML kaynak yönetimi (default / kullanıcı)
│   ├── LicenseChecker.cs          # API üzerinden lisans doğrulama
│   ├── LogManager.cs              # Günlük log sistemi
│   ├── XmlSyntaxChecker.cs        # XML sözdizimi kontrolü ve marker sistemi
│   ├── XsltCompletionProvider.cs  # Otomatik tamamlama (XSLT + UBL tag'leri)
│   └── XsltTransformer.cs         # XSLT 1.0 / 2.0 dönüşüm motoru
├── Forms/
│   ├── EditorForm.cs              # Ana editör ekranı
│   └── LicenseForm.cs             # Lisans hata ekranı
├── Database/
│   └── credentials.db             # Şifreli kimlik & token veritabanı (otomatik oluşturulur)
├── UBL/
│   ├── FATubl.xml                 # Default e-Fatura örnek XML
│   └── IRSubl.xml                 # Default e-İrsaliye örnek XML
├── TEMP/                          # Kullanıcı XML'leri geçici olarak burada tutulur
├── LOG/                           # Günlük log dosyaları
├── Program.cs                     # Uygulama girişi + lisans kontrolü
└── app.config                     # Lisans API URL ayarı
```

---

## 🔒 Lisanslama Sistemi

Uygulama başlatıldığında **donanım ID'si** (CPU + Anakart + BIOS seri numaralarından SHA256) oluşturulur ve lisans sunucusuna sorgulanır.

- ✅ Lisans geçerliyse → Editör açılır
- ❌ Lisans yoksa → Hardware ID gösterilir, yetkili ile iletişime yönlendirilir

**Lisans API** ayrı bir .NET 8 Web API projesi olarak sunucuda Windows servisi olarak çalışmaktadır.

---

## 🗝️ Kimlik Bilgisi & Token Yönetimi (CredentialManager)

`CredentialManager` sınıfı, kullanıcı kimlik bilgilerini ve oturum token'larını **AES-256-GCM** şifrelemesiyle **SQLite** veritabanında güvenli biçimde saklar.

### Veritabanı Yapısı

```
Database/credentials.db
├── settings   → Anahtar-değer çiftleri (şifreli)
└── tokens     → Oturum token'ları (şifreli, tek kayıt)
```

### Şifreleme

| Özellik | Detay |
|---|---|
| Algoritma | AES-256-GCM (BouncyCastle) |
| Anahtar uzunluğu | 256 bit (32 byte) |
| Nonce | 12 byte, her şifreleme için rastgele üretilir |
| Auth Tag | 128 bit |

- Her `Encrypt` çağrısında benzersiz nonce üretilir; aynı veri her seferinde farklı şifre üretir.
- Nonce, şifreli veriyle birlikte Base64 olarak saklanır (`nonce [12B] + ciphertext + tag`).

### Token Yönetimi

| Metot | Açıklama |
|---|---|
| `SaveToken(token, expireDate, server, firm)` | Mevcut token'ı silerek yeni token kaydeder |
| `GetValidToken()` | Geçerli token döndürür; süresi dolmuşsa (5 dk erken) `null` döner |
| `ClearToken()` | Token tablosunu temizler |

Token süresi dolmadan **5 dakika önce** geçersiz sayılır; bu sayede sunucuya süresi dolmuş token gönderilmez.

### Ayar Yönetimi

```csharp
CredentialManager.Save("kullanici_adi", "örnek_değer"); // Şifreli kaydet
string deger = CredentialManager.Load("kullanici_adi"); // Çözerek oku
```

### Kullanılan Paket

```
Portable.BouncyCastle   (Org.BouncyCastle.Crypto)
System.Data.SQLite
```

---

## 🛠️ Kurulum & Çalıştırma

### Gereksinimler

- Windows 10/11 (x64)
- .NET Framework 4.8
- Visual Studio 2022

### Adımlar

1. **Projeyi Klonla:**
   ```bash
   git clone https://github.com/dogukankosan/XSLTEditor.git
   cd XSLTEditor
   ```

2. **NuGet Paketlerini Yükle:**
   ```
   CefSharp.WinForms         v119.4.30
   ICSharpCode.TextEditor    v3.2.1
   Saxon-HE                  v10.9.0
   DevExpress                v20.1
   System.Data.SQLite        (latest)
   Portable.BouncyCastle     (latest)
   ```

3. **`app.config` Dosyasını Güncelle:**
   ```xml
   <appSettings>
     <add key="LicenseApiUrl" value="http://SUNUCU_IP:9922" />
   </appSettings>
   ```

4. **Projeyi Visual Studio ile Aç ve Çalıştır (`F5`):**
   - Lisans kontrolü yapılır
   - Geçerliyse editör açılır
   - XSLT dosyasını aç, XML seç, F5 ile önizle

---

## ⚡ Kullanım Senaryosu

1. Uygulamayı başlat — lisans otomatik kontrol edilir
2. **XSLT Aç** butonuyla şablon dosyasını yükle
3. Belge türü otomatik algılanır (e-Fatura / e-İrsaliye)
4. Editörde düzenleme yap — sözdizimi hatası varsa anında işaretlenir
5. **F5** veya **Yenile** ile önizleme panelinde HTML çıktısını gör
6. **Ctrl+S** ile kaydet, **Farklı Kaydet** ile yeni dosyaya aktar
7. İstersen kendi XML'ini yükle, default XML'e dön

---

## ⌨️ Kısayollar

| Kısayol | İşlev |
|---|---|
| `Ctrl+S` | Kaydet |
| `F5` | Önizlemeyi Yenile |
| `Ctrl+Space` | Otomatik Tamamlama |
| `Ctrl+F` | Ara |

---

## 🧱 Kullanılan Teknolojiler

| Teknoloji | Açıklama |
|---|---|
| .NET Framework 4.8 | Ana platform |
| CefSharp 119 | HTML önizleme tarayıcısı |
| Saxon-HE 10.9 | XSLT 2.0/3.0 motoru |
| ICSharpCode TextEditor | Kod editörü |
| DevExpress 20.1 | Ribbon arayüzü |
| System.Management | WMI donanım bilgisi |
| System.Data.SQLite | Yerel veritabanı |
| Portable.BouncyCastle | AES-256-GCM şifreleme |

---

## 🤝 Katkı

Katkı sağlamak için projeyi forklayabilir ve pull request gönderebilirsiniz.

---

## 📄 Lisans

MIT License

---

## 📬 İletişim

- 👨‍💻 Geliştirici: [@dogukankosan](https://github.com/dogukankosan)
- 🐞 Öneri veya sorun: [Issues sekmesi](https://github.com/dogukankosan/XSLTEditor/issues)

---

<p align="center">
  <img src="https://img.shields.io/badge/.NET-Framework%204.8-blue?logo=dotnet" alt="dotnet" />
  <img src="https://img.shields.io/badge/Windows%20Forms-UI-lightgrey" alt="winforms" />
  <img src="https://img.shields.io/badge/CefSharp-119.4.30-green" alt="cefsharp" />
  <img src="https://img.shields.io/badge/Saxon--HE-10.9.0-orange" alt="saxon" />
  <img src="https://img.shields.io/badge/DevExpress-20.1-purple" alt="devexpress" />
  <img src="https://img.shields.io/badge/AES--256--GCM-BouncyCastle-red" alt="bouncy" />
  <img src="https://img.shields.io/badge/SQLite-credentials-blue" alt="sqlite" />
</p>
