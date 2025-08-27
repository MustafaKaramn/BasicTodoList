# Gelişmiş TodoList API Projesi

Bu proje, modern .NET teknolojileri ve yazılım mimarisi pratikleri kullanılarak geliştirilmiş bir yapılacaklar listesi uygulamasıdır. Proje, bir junior developer olarak kendimi geliştirmek ve profesyonel yetkinliklerimi pratik etme amacıyla geliştirilmektedir.

---

## 🎯 Projenin Amacı

Bu projenin temel amacı, katmanlı mimari, tasarım desenleri ve modern API geliştirme tekniklerini uygulayarak sağlam, ölçeklenebilir ve bakımı kolay bir backend sistemi oluşturmaktır. Proje, aşağıdaki yetkinlikleri pratik etmek üzere tasarlanmıştır:

* Temiz kod (Clean Code) ve SOLID prensiplerine uygunluk.
* Profesyonel bir proje yapısı ve mimari kurma becerisi.
* Gerçek dünya problemlerine (dosya yönetimi, veri bütünlüğü, hata yönetimi) modern çözümler üretme.
* Güvenli, performanslı ve esnek API endpoint'leri tasarlama.

---

## ✨ Temel Özellikler

* **Liste Yönetimi:** Birden fazla yapılacaklar listesi (`TodoList`) oluşturma, güncelleme, silme ve listeleme.
* **Görev Yönetimi:** Listelere görev (`TodoItem`) ekleme, güncelleme, silme ve listeleme.
* **Çoka Çok İlişki:** Bir görevin birden fazla listeye atanabilmesi.
* **Dosya Yükleme:** Her `TodoList` için bir kapak fotoğrafı yükleme, güncelleme ve silme.
* **Gelişmiş Pagination:** `TodoItem` listeleme endpoint'lerinde meta veri (toplam sayfa, toplam kayıt sayısı, sonraki/önceki sayfa durumu vb.) içeren gelişmiş sayfalama.
* **Dinamik Filtreleme:** Görevleri tamamlanma durumuna (`status`) göre dinamik olarak filtreleme.
* **Merkezi Hata Yönetimi:** Uygulama genelinde oluşan hataları yakalayan ve standart bir formatta istemciye sunan Global Exception Handling Middleware.
* **Yapısal Günlükleme (Structured Logging):** Serilog ile tüm önemli olayların ve hataların yapısal (JSON) olarak dosyaya kaydedilmesi.

---

## 🏗️ Mimari ve Tasarım Desenleri

Proje, sorumlulukların net bir şekilde ayrıldığı **4 Katmanlı Mimari (N-Tier Architecture)** üzerine inşa edilmiştir:

* **`Core` Katmanı:** Projenin en temel katmanıdır. Entity'leri ve tüm katmanlarca paylaşılan yardımcı sınıfları içerir. Hiçbir katmana bağımlılığı yoktur.
* **`DataAccess` Katmanı:** Veritabanı işlemlerinden sorumludur. Entity Framework Core ve veritabanı ile ilgili tüm kodları barındırır.
* **`Business` Katmanı:** Uygulamanın iş mantığı, kuralları ve validasyonları bu katmanda yer alır. DTO'lar ve servisler burada bulunur.
* **`API` Katmanı:** Dış dünyaya açılan kapıdır. Gelen HTTP isteklerini karşılar, ilgili Business servisine yönlendirir ve sonucu istemciye döndürür.

Bu mimariyi desteklemek için aşağıdaki **Tasarım Desenleri ve Yaklaşımlar** kullanılmıştır:

* **Repository Pattern:** Veri erişim mantığını iş katmanından soyutlayarak veritabanı operasyonlarını merkezileştirir.
* **Generic Repository:** CRUD operasyonları için kod tekrarını önleyen jenerik bir depo yapısıdır.
* **Unit of Work Pattern:** Birden fazla veritabanı operasyonunun tek bir transaction olarak yönetilmesini sağlayarak veri bütünlüğünü garanti eder.
* **Service Layer:** İş mantığını Repository'lerden ve Controller'lardan ayırarak daha temiz ve test edilebilir bir yapı sunar.
* **Dependency Injection (DI):** Katmanlar ve sınıflar arasındaki bağımlılıkları gevşeterek esnek ve test edilebilir bir sistem oluşturur.
* **DTO (Data Transfer Object) Pattern:** Katmanlar arasında (özellikle API ve istemci) veri taşımak, güvenlik sağlamak ve API kontratını korumak için kullanılır.
* **AutoMapper:** Entity ve DTO nesneleri arasındaki dönüşümleri otomatikleştirerek kod tekrarını azaltır.

---

## 🛠️ Kullanılan Teknolojiler ve Kütüphaneler

### Backend
* **.NET 9**
* **ASP.NET Core Web API**
* **Entity Framework Core 9**
* **Serilog:** Yapısal günlükleme için.
* **AutoMapper:** Nesne-nesne haritalama için.
* **Moq & FluentAssertions:** Birim testleri için.

### Veritabanı
* **MS SQL Server**

### Test
* **xUnit:** Test çatısı olarak.

---

## 🚀 API Endpoint'leri

Aşağıda projedeki temel endpoint'lerden bazıları listelenmiştir:

| Metot  | URL                                           | Açıklama                                                                          |
| :----- | :-------------------------------------------- | :-------------------------------------------------------------------------------- |
| `GET`  | `/api/TodoLists`                              | Tüm listeleri getirir.                                                            |
| `GET`  | `/api/TodoLists/{id}`                         | Belirtilen ID'ye sahip listeyi getirir.                                           |
| `POST` | `/api/TodoLists`                              | Yeni bir liste oluşturur. `multipart/form-data` olarak fotoğraf da kabul eder.    |
| `GET`  | `/api/TodoItems`                              | Tüm görevleri getirir. `status`, `pageNumber` ve `pageSize` ile filtreleme/sayfalama destekler. |
| `POST` | `/api/TodoItems`                              | Yeni bir görev oluşturur.                                                         |

---

## ⚙️ Kurulum ve Çalıştırma

Projeyi yerel makinenizde çalıştırmak için aşağıdaki adımları izleyin:

1.  **Repository'yi Klonlayın:**
    ```bash
    git clone https://github.com/MustafaKaramn/BasicTodoList.git
    ```
2.  **Proje Dizisine Gidin:**
    ```bash
    cd TodoList
    ```
3.  **.NET Bağımlılıklarını Yükleyin:**
    ```bash
    dotnet restore
    ```
4.  **Veritabanı Bağlantısını Yapılandırın:**
    * `TodoList.API` projesi içindeki `appsettings.json` dosyasını açın.
    * `ConnectionStrings` bölümündeki `DefaultConnection` değerini kendi MS SQL Server bağlantı cümlenizle değiştirin.
5.  **Veritabanını Oluşturun ve Güncelleyin:**
    * Package Manager Console'da `Default project` olarak `TodoList.DataAccess`'i seçin.
    * Aşağıdaki komutu çalıştırın:
        ```powershell
        Update-Database
        ```
6.  **Projeyi Çalıştırın:**
    * `TodoList.API` projesini başlangıç projesi olarak ayarlayın ve `F5`'e basın veya aşağıdaki komutu çalıştırın:
        ```bash
        dotnet run --project TodoList.API
        ```
    * Uygulama başladığında, Swagger arayüzüne `https://localhost:5122/swagger` adresinden erişebilirsiniz.

---

## 🔮 Gelecek Planları ve Geliştirmeler

* [ ] **Authentication & Authorization:** JWT (JSON Web Tokens) ile kullanıcı girişi ve rol tabanlı yetkilendirme.
* [ ] **Unit & Integration Test:** Kod kapsamını (code coverage) artırmak için daha fazla birim ve entegrasyon testi yazılması.
* [ ] **Caching:** Sık istenen veriler için Redis gibi bir önbellekleme mekanizması eklenmesi.
* [ ] **CI/CD Pipeline:** GitHub Actions ile otomatik build, test ve deploy süreçlerinin oluşturulması.

---

## 📬 İletişim

Mustafa Karaman

* GitHub: `@MustafaKaramn`
