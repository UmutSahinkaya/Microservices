# 🚀 Microservices E-Learning Platform

> **Modern .NET tabanlı, tam özellikli bir online kurs satış platformu — Microservices mimarisi ile geliştirilmiştir.**

[![.NET 5](https://img.shields.io/badge/.NET-5.0-512BD4?style=flat&logo=dotnet)](https://dotnet.microsoft.com/)
[![MongoDB](https://img.shields.io/badge/MongoDB-4EA94B?style=flat&logo=mongodb&logoColor=white)](https://www.mongodb.com/)
[![Redis](https://img.shields.io/badge/Redis-DC382D?style=flat&logo=redis&logoColor=white)](https://redis.io/)
[![RabbitMQ](https://img.shields.io/badge/RabbitMQ-FF6600?style=flat&logo=rabbitmq&logoColor=white)](https://www.rabbitmq.com/)
[![IdentityServer4](https://img.shields.io/badge/IdentityServer4-orange?style=flat)](https://identityserver4.readthedocs.io/)
[![Ocelot](https://img.shields.io/badge/Ocelot-API_Gateway-blueviolet?style=flat)](https://github.com/ThreeMammals/Ocelot)

---

## 📖 İçindekiler

- [Proje Hakkında](#-proje-hakkında)
- [Mimari](#️-mimari)
- [Teknolojiler](#️-teknolojiler)
- [Servisler](#-servisler)
- [Kurulum](#-kurulum)
- [Veritabanı Yapılandırması](#️-veritabanı-yapılandırması)
- [Projeyi Çalıştırma](#-projeyi-çalıştırma)
- [Servisler Arası İletişim](#-servisler-arası-iletişim)
- [Authentication & Authorization](#-authentication--authorization)
- [API Gateway](#-api-gateway)
- [Kullanım Senaryoları](#-kullanım-senaryoları)
- [Klasör Yapısı](#-klasör-yapısı)
- [Design Patterns](#-design-patterns)
- [API Endpoints](#-api-endpoints)

---

## 🎯 Proje Hakkında

Bu proje, **modern microservices mimarisi** kullanılarak geliştirilmiş bir **online eğitim platformudur**. Kullanıcılar kurs satın alabilir, sepete ekleyebilir, ödeme yapabilir ve siparişlerini takip edebilir. Her bir servis bağımsız olarak çalışır, farklı veritabanları kullanır ve birbirleriyle **senkron (HTTP)** veya **asenkron (RabbitMQ)** iletişim kurar.

### 🌟 Temel Özellikler

- ✅ **Microservices Mimarisi** — Her servis bağımsız, ölçeklenebilir
- ✅ **API Gateway Pattern** — Ocelot ile merkezi yönlendirme
- ✅ **Authentication & Authorization** — IdentityServer4 ile JWT token bazlı güvenlik
- ✅ **Multiple Database** — MongoDB, SQL Server, Redis
- ✅ **Event-Driven Communication** — RabbitMQ ile asenkron mesajlaşma
- ✅ **Domain Driven Design (DDD)** — Order servisi için DDD pattern
- ✅ **CQRS & MediatR** — Komut ve sorgu ayrımı
- ✅ **Token Exchange** — Güvenli servisler arası yetkilendirme
- ✅ **Photo Storage** — Ayrı fotoğraf saklama servisi

---

## 🏗️ Mimari

### Sistem Mimarisi Diyagramı

```
┌─────────────────────────────────────────────────────────────────────────┐
│                           KULLANICI (Browser)                           │
└────────────────────────────────┬────────────────────────────────────────┘
                                 │
                                 ▼
┌─────────────────────────────────────────────────────────────────────────┐
│                    WEB MVC CLIENT (Port: 5005)                          │
│   ASP.NET Core MVC · Cookie Authentication · Razor Views                │
└────────────┬───────────────────┬────────────────────┬───────────────────┘
             │                   │                    │
             ▼                   ▼                    ▼
┌────────────────────┐  ┌──────────────────┐  ┌────────────────────────┐
│  IdentityServer4   │  │   API Gateway    │  │   PhotoStock Direct    │
│   (Port: 5001)     │  │  (Port: 5000)    │  │    (Port: 5012)        │
│                    │  │                  │  │                        │
│ · User Management  │  │ · Ocelot         │  │ · Static File Serving  │
│ · JWT Token        │  │ · Routing        │  │ · Photo Upload/Delete  │
│ · Refresh Token    │  │ · Authentication │  │                        │
│ · SQL Server       │  │ · Token Exchange │  └────────────────────────┘
└────────────────────┘  └────────┬─────────┘
                                 │
                 ┌───────────────┼───────────────┐
                 │               │               │
                 ▼               ▼               ▼
        ┌────────────┐  ┌────────────┐  ┌────────────┐
        │  Catalog   │  │   Basket   │  │  Discount  │
        │(Port:5011) │  │(Port:5013) │  │(Port:5014) │
        │  MongoDB   │  │   Redis    │  │  SQL / PG  │
        └─────┬──────┘  └────────────┘  └────────────┘
              │
              │ 🐰 RabbitMQ (Async Event)
              ▼
        ┌────────────┐  ┌────────────┐
        │   Order    │  │  Payment   │
        │(Port:5015) │  │(Port:5016) │
        │ DDD · CQRS │  │ Fake Pay   │
        │ SQL Server │  │            │
        └────────────┘  └────────────┘
```

---

## 🛠️ Teknolojiler

| Katman | Teknoloji |
|--------|-----------|
| **Framework** | .NET 5, .NET Core 3.1, .NET Standard 2.1 |
| **Web UI** | ASP.NET Core MVC, Razor Views, Bootstrap |
| **API** | ASP.NET Core Web API |
| **Auth** | IdentityServer4, JWT Bearer, ASP.NET Core Identity |
| **Gateway** | Ocelot |
| **NoSQL DB** | MongoDB |
| **SQL DB** | SQL Server (Identity, Order) |
| **Cache** | Redis (Basket) |
| **Message Broker** | RabbitMQ + MassTransit |
| **ORM** | Entity Framework Core, MongoDB.Driver |
| **Mapping** | AutoMapper |
| **Validation** | FluentValidation |
| **CQRS** | MediatR |

---

## 📦 Servisler

### 1️⃣ Identity Server — Port 5001

Tüm sistemin **kimlik ve yetkilendirme** merkezi. OAuth 2.0 ve OpenID Connect protokollerini uygular.

- Kullanıcı kaydı ve girişi
- JWT Access Token & Refresh Token üretimi
- Token doğrulama
- Kullanıcı rolleri ve claim yönetimi

**Veritabanı:** SQL Server

**Tanımlı Client'lar:**

| Client | Grant Type | Kullanım |
|--------|-----------|----------|
| `WebMvcClient` | Client Credentials | Makine-makine iletişimi (Catalog, PhotoStock) |
| `WebMvcClientForUser` | Resource Owner Password | Kullanıcı login (Basket, Order) |
| `TokenExchangeClient` | Token Exchange | Gateway → Discount/Payment |

**Tanımlı API Scope'ları:**
```
catalog_fullpermission, basket_fullpermission, order_fullpermission,
discount_fullpermission, payment_fullpermission, photo_stock_fullpermission,
gateway_fullpermission
```

---

### 2️⃣ API Gateway — Port 5000

Tüm microservice'lere **tek giriş noktası**. Ocelot kütüphanesi ile routing, authentication ve token exchange işlemlerini yönetir.

**Route Tablosu:**

| Upstream (Dışarıdan gelen) | Downstream (Hedefteki servis) |
|----------------------------|-------------------------------|
| `/services/catalog/{everything}` | `localhost:5011/api/{everything}` |
| `/services/photostock/{everything}` | `localhost:5012/api/{everything}` |
| `/services/basket/{everything}` | `localhost:5013/api/{everything}` |
| `/services/discount/{everything}` | `localhost:5014/api/{everything}` |
| `/services/order/{everything}` | `localhost:5015/api/{everything}` |
| `/services/payment/{everything}` | `localhost:5016/api/{everything}` |

**Token Exchange:**
Discount ve Payment servisleri için Gateway, kullanıcı token'ını `TokenExchangeClient` aracılığıyla daha dar scope'lu bir servise özel token'a çevirir.

---

### 3️⃣ Catalog Service — Port 5011

Kurs ve kategori yönetimi. **MongoDB** üzerinde çalışır.

- Kurs CRUD işlemleri
- Kullanıcıya ait kursları listeleme
- Kurs güncellendiğinde **RabbitMQ'ya event yayınlama**

**MongoDB Koleksiyonları:** `CatalogDb → Courses, Categories`

**RabbitMQ Event:**
```csharp
// Kurs adı değiştiğinde Order Service'e bildirim gönderir
CourseNameChangedEvent { CourseId, UpdatedName }
```

---

### 4️⃣ Basket Service — Port 5013

Kullanıcı sepet yönetimi. **Redis** (In-Memory) üzerinde çalışır.

- Sepete ürün ekleme/çıkarma
- İndirim kodu uygulama
- Sipariş sonrası sepeti temizleme

**Redis Key Yapısı:** `basket:{userId} → JSON`

---

### 5️⃣ Order Service — Port 5015

Sipariş yönetimi. **DDD + CQRS** mimarisi ile geliştirilmiş, **SQL Server** üzerinde çalışır.

**Katmanlı Mimari:**
```
Services.Order.Domain.Core    → Base class'lar (Entity, ValueObject)
Services.Order.Domain         → Domain modelleri (Order, OrderItem, Address)
Services.Order.Application    → Use case'ler, CQRS Handler'lar, RabbitMQ Consumer'lar
Services.Order.Infrastructure → EF Core, DbContext
Services.Order.API            → Controller'lar, Startup
```

**RabbitMQ Consumer:**
```csharp
// Catalog'dan gelen CourseNameChangedEvent'i dinler
// Sipariş kalemlerinde kurs adını günceller
CourseNameChangedEventConsumer : IConsumer<CourseNameChangedEvent>
```

---

### 6️⃣ Discount Service — Port 5014

İndirim kuponları yönetimi. Kullanıcıya özel indirim kodları üretir ve doğrular.

> ⚠️ Bu servise erişim **Token Exchange** ile yapılır.

---

### 7️⃣ FakePayment Service — Port 5016

Test amaçlı sahte ödeme servisi. Kredi kartı bilgilerini simüle eder.

> ⚠️ Bu servise erişim **Token Exchange** ile yapılır.

---

### 8️⃣ PhotoStock Service — Port 5012

Kurs fotoğraflarını fiziksel dosya sistemi üzerinde saklar.

- Fotoğraf upload → `wwwroot/photos/{guid}.jpg`
- Fotoğraf delete
- Static file serving → `http://localhost:5012/photos/{filename}`

**Önemli Akış:**
1. Kurs oluşturulurken → **önce** fotoğraf PhotoStock'a yüklenir
2. Dönen URL → Catalog Service'e kursun `Picture` alanı olarak gönderilir
3. Kurs listelenirken → `PhotoHelper` ile tam URL oluşturulur

---

### 9️⃣ Web MVC — Port 5005

Kullanıcı arayüzü. Tüm servisleri **orchestrate** eder.

- ASP.NET Core MVC + Razor Views
- Cookie Authentication
- FluentValidation
- `ResourceOwnerPasswordTokenHandler` — Token'ı request'e ekler, expired ise refresh eder
- `ClientCredentialTokenHandler` — Client credentials token yönetimi

---

### 🔟 Shared Library

Tüm servislerin ortak kullandığı yapılar:

- `Response<T>` — Standart API response wrapper
- `CustomBaseController` — Ortak controller base class
- `SharedIdentityService` — Token'dan kullanıcı ID çıkarma
- `CourseNameChangedEvent` — Servisler arası event sözleşmesi

---

## 📥 Kurulum

### Gereksinimler

| Araç | Açıklama |
|------|----------|
| [.NET 5 SDK](https://dotnet.microsoft.com/download/dotnet/5.0) | Ana framework |
| [MongoDB](https://www.mongodb.com/try/download/community) | Catalog veritabanı |
| [SQL Server](https://www.microsoft.com/tr-tr/sql-server/sql-server-downloads) | Identity & Order veritabanı |
| [Redis](https://redis.io/download) | Basket cache |
| [RabbitMQ](https://www.rabbitmq.com/download.html) | Message broker |

### Projeyi Klonlayın

```bash
git clone https://github.com/UmutSahinkaya/Microservices.git
cd Microservices
dotnet restore
```

---

## 🗄️ Veritabanı Yapılandırması

### MongoDB (Catalog Service)

```bash
# Docker ile
docker run -d --name mongodb -p 27017:27017 mongo

# Mongo Shell → Örnek kategori
mongo
> use CatalogDb
> db.Categories.insertOne({ "Name": "Programlama" })
```

**`appsettings.json`:**
```json
{
  "DatabaseSettings": {
    "ConnectionString": "mongodb://localhost:27017",
    "DatabaseName": "CatalogDb",
    "CourseCollectionName": "Courses",
    "CategoryCollectionName": "Categories"
  }
}
```

### SQL Server (Identity & Order)

```bash
# Docker ile
docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=Password12*" \
  -p 1444:1433 -d mcr.microsoft.com/mssql/server:2019-latest
```

```bash
# Identity DB Migration
cd IdentityServer/IdentityServer
dotnet ef database update

# Order DB Migration
cd Services/Order/Services.Order.API
dotnet ef database update
```

### Redis (Basket Service)

```bash
# Docker ile
docker run -d --name redis -p 6379:6379 redis
```

### RabbitMQ

```bash
# Docker ile (Management UI dahil)
docker run -d --name rabbitmq -p 5672:5672 -p 15672:15672 rabbitmq:3-management
# UI: http://localhost:15672 — guest / guest
```

---

## 🏃 Projeyi Çalıştırma

### Sıralı Başlatma

> **IdentityServer mutlaka ilk başlamalıdır** çünkü diğer servisler token doğrulaması için ona bağımlıdır.

| Sıra | Servis | Port | Komut |
|------|--------|------|-------|
| 1 | IdentityServer | 5001 | `cd IdentityServer/IdentityServer && dotnet run` |
| 2 | Gateway | 5000 | `cd Gateways/Gateway && dotnet run` |
| 3 | Catalog | 5011 | `cd Services/Catalog/Services.Catalog && dotnet run` |
| 4 | PhotoStock | 5012 | `cd Services/PhotoStock/Services.PhotoStock && dotnet run` |
| 5 | Basket | 5013 | `cd Services/Basket/Services.Basket && dotnet run` |
| 6 | Discount | 5014 | `cd Services/Discount/Services.Discount && dotnet run` |
| 7 | Order | 5015 | `cd Services/Order/Services.Order.API && dotnet run` |
| 8 | Payment | 5016 | `cd Services/FakePayment/Services.FakePayment && dotnet run` |
| 9 | Web MVC | 5005 | `cd FrontEnds/Web && dotnet run` |

### Visual Studio ile

1. Solution'a sağ tıklayın → **Properties**
2. **Multiple startup projects** seçin
3. Yukarıdaki sırayla tüm projeleri **Start** yapın
4. **F5** ile çalıştırın

---

## 🔄 Servisler Arası İletişim

### Senkron HTTP (Request-Response)

Kullanıcı bir sayfayı açtığında **anında yanıt** gereken durumlarda kullanılır.

**Örnek — Kurs Listeleme:**
```
Browser → Web MVC → Gateway (5000) → Catalog Service (5011) → MongoDB
                                              ↓
Browser ← Web MVC ← Gateway (5000) ← Catalog Service (5011)
```

**Örnek — Kurs Oluşturma (2 servis birlikte):**
```
Web MVC
  │
  ├─ 1. POST PhotoStock → Fotoğrafı yükle → "guid.jpg" URL'i döner
  │
  └─ 2. POST Catalog    → Kurs bilgisi + fotoğraf URL'i → MongoDB'ye kaydet
```

**Örnek — Sipariş Oluşturma (4 servis sırayla):**
```
Web MVC (Orchestrator)
  │
  ├─ 1. GET  Basket Service  → Sepeti al
  ├─ 2. POST Payment Service → Ödeme yap (Token Exchange)
  ├─ 3. POST Order Service   → Sipariş oluştur
  └─ 4. DELETE Basket Service → Sepeti temizle
```

### Asenkron Event-Driven (RabbitMQ)

Kullanıcının **anında yanıt beklemediği**, servisler arası veri tutarlılığı gerektiren durumlarda kullanılır.

**Örnek — Kurs Adı Güncellenince:**
```
Catalog Service                    Order Service
     │                                  │
     │  1. Kurs adı güncellendi         │
     │     (MongoDB güncellendi)        │
     │                                  │
     │  2. CourseNameChangedEvent       │
     │────── RabbitMQ ─────────────────>│
     │                                  │
     │                    3. Sipariş kalemlerinde
     │                       kurs adını güncelle
     │                       (SQL Server güncellendi)
```

**Neden iki farklı yöntem var?**

| | Senkron HTTP | Asenkron RabbitMQ |
|--|-------------|-------------------|
| **Ne zaman?** | Kullanıcı hemen sonuç bekliyor | Eventual consistency yeterli |
| **Hata durumu** | Anında hata gösterilebilir | Retry mekanizması ile tekrar denenebilir |
| **Bağımlılık** | Hedef servis çalışmalı | Hedef servis çökse bile mesaj kuyrukta bekler |

---

## 🔐 Authentication & Authorization

### 3 Katmanlı Güvenlik

```
1. WEB MVC        → Cookie Authentication (Login kontrolü)
2. API GATEWAY    → JWT Bearer + Scope kontrolü
3. MICROSERVICE   → JWT Bearer (kendi audience kontrolü)
```

### Login Akışı

```
Kullanıcı
  │ email + password
  ▼
Web MVC
  │ POST /connect/token (Resource Owner Password Flow)
  ▼
IdentityServer
  │ Kullanıcıyı doğrula (SQL Server)
  │ Access Token + Refresh Token üret
  ▼
Web MVC
  │ Token'ları Cookie'ye kaydet
  ▼
Kullanıcı (Giriş yapıldı ✅)
```

### Token Yaşam Döngüsü

```
1. Access Token süresi: 1 saat
2. Refresh Token süresi: 60 gün
3. Token expired olduğunda:
   ResourceOwnerPasswordTokenHandler otomatik olarak
   Refresh Token ile yeni Access Token alır
```

### Token Exchange (Discount & Payment için)

```
Kullanıcı Token                 Servis Token
(basket_fullpermission)         (discount_fullpermission)
         │                              ▲
         │    Gateway                   │
         └──> TokenExchangeHandler ─────┘
              IdentityServer'dan yeni
              scope'lu token alır
```

---

## 🌐 API Gateway

### Ocelot Routing

İstemciden gelen istek Gateway'e ulaşır. Ocelot URL'yi eşleştirir ve hedef servise yönlendirir.

```
İstek:  GET http://localhost:5000/services/catalog/courses
                                  ↓
Ocelot: UpstreamPathTemplate = /services/catalog/{everything}
        {everything} = "courses"
                                  ↓
Hedef:  GET http://localhost:5011/api/courses
```

### Authentication Pipeline

```
1. İstek Gateway'e ulaşır
2. JWT token doğrulanır (IdentityServer'a sorulur)
3. Token'daki scope kontrol edilir (AllowedScopes)
4. Token Exchange gerekiyorsa handler çalışır
5. İstek downstream servisine yönlendirilir
```

---

## 🎬 Kullanım Senaryoları

### Senaryo 1 — Kullanıcı Kurslarını Görüntülüyor

```
1. GET /Courses/Index
2. Cookie'den Access Token alınır
3. ResourceOwnerPasswordTokenHandler token'ı header'a ekler
4. GET http://localhost:5000/services/catalog/courses/GetAllByUserId/{userId}
5. Gateway → JWT doğrulama ✅ → Scope kontrolü ✅
6. GET http://localhost:5011/api/courses/GetAllByUserId/{userId}
7. Catalog Service → MongoDB'den kursları çeker
8. Web MVC → PhotoHelper ile fotoğraf URL'lerini oluşturur
   "guid.jpg" → "http://localhost:5012/photos/guid.jpg"
9. Razor View render edilir → HTML kullanıcıya döner
```

### Senaryo 2 — Yeni Kurs Oluşturma

```
1. POST /Courses/Create (form + fotoğraf)
2. CatalogService.CreateCourseAsync() çalışır:
   a. PhotoStockService.UploadPhoto(file)
      → POST http://localhost:5012/api/photos
      → Fotoğraf wwwroot/photos/guid.jpg olarak kaydedilir
      → "guid.jpg" döner
   b. courseCreateInput.Picture = "guid.jpg"
   c. POST http://localhost:5000/services/catalog/courses
      → Catalog Service → MongoDB'ye kurs kaydedilir
3. Redirect → /Courses/Index
```

### Senaryo 3 — Sipariş Verme

```
1. POST /Order/Checkout
2. OrderService.CreateOrder() çalışır (Orchestrator):
   a. BasketService.Get() → Redis'ten sepet
   b. PaymentService.ReceivePayment() → Ödeme (Token Exchange)
   c. OrderService → POST /api/orders → SQL Server'a sipariş
   d. BasketService.Delete() → Redis'ten sepet silinir
3. Sipariş başarılı sayfası gösterilir
```

### Senaryo 4 — Kurs Adı Güncelleme (Asenkron)

```
1. PUT /Courses/Update
2. Catalog Service → MongoDB'de kurs güncellenir
3. Catalog Service → RabbitMQ'ya CourseNameChangedEvent publish
4. Order Service → Consumer event'i alır
5. Order Service → SQL Server'da sipariş kalemlerini günceller
6. Veriler eventual consistency ile senkronize olur ✅
```

---

## 📁 Klasör Yapısı

```
Microservices/
│
├── FrontEnds/
│   └── Web/                            # ASP.NET Core MVC Web Uygulaması
│       ├── Controllers/                # Auth, Courses, Baskets, Order, Home, User
│       ├── Models/                     # ViewModel'ler & Input modelleri
│       ├── Services/                   # Servis HttpClient implementasyonları
│       ├── Handler/                    # Token handler'lar (DelegatingHandler)
│       ├── Helpers/                    # PhotoHelper
│       ├── Validators/                 # FluentValidation kuralları
│       ├── Views/                      # Razor view'lar
│       └── appsettings.json
│
├── Gateways/
│   └── Gateway/                        # Ocelot API Gateway
│       ├── DelegateHandlers/           # TokenExchangeDelegateHandler
│       └── configuration.development.json  # Route tanımları
│
├── IdentityServer/
│   └── IdentityServer/                 # IdentityServer4
│       ├── Config.cs                   # Client, Scope, Resource tanımları
│       ├── Controllers/               # UserController
│       ├── Data/                      # ApplicationDbContext
│       └── Models/                    # ApplicationUser
│
├── Services/
│   ├── Catalog/Services.Catalog/       # MongoDB · Kurs & Kategori
│   ├── Basket/Services.Basket/         # Redis · Sepet yönetimi
│   ├── PhotoStock/Services.PhotoStock/ # File System · Fotoğraf saklama
│   ├── Discount/Services.Discount/     # İndirim kuponu yönetimi
│   ├── FakePayment/Services.FakePayment/ # Sahte ödeme simülasyonu
│   └── Order/                          # DDD Katmanlı Mimari
│       ├── Services.Order.Domain.Core/ # Entity, ValueObject base class
│       ├── Services.Order.Domain/      # Aggregate Root, Domain modeller
│       ├── Services.Order.Application/ # CQRS Handler, Consumer
│       ├── Services.Order.Infrastructure/ # EF Core DbContext
│       └── Services.Order.API/         # Controller, Startup
│
└── Shared/Shared/                      # Ortak kütüphane
    ├── ControllerBases/                # CustomBaseController
    ├── Dtos/                           # Response<T>, NoContent
    ├── Messages/                       # CourseNameChangedEvent
    └── Services/                       # SharedIdentityService
```

---

## 🎨 Design Patterns

| Pattern | Kullanıldığı Yer | Açıklama |
|---------|------------------|----------|
| **Microservices** | Tüm proje | Her servis bağımsız deploy edilebilir, kendi DB'si var |
| **API Gateway** | Ocelot | Tek giriş noktası, routing, auth merkezi |
| **Event-Driven** | Catalog → Order | RabbitMQ ile asenkron iletişim |
| **CQRS** | Order Service | Komut ve sorgu ayrımı (MediatR) |
| **DDD** | Order Service | Aggregate Root, Value Object, Domain Event |
| **Repository** | Catalog, Order | Data access soyutlaması |
| **Delegation Handler** | Web MVC, Gateway | Token yenileme, Token Exchange |
| **Orchestration** | Web MVC | Sipariş akışında 4 servisi sırayla çağırma |
| **Database per Service** | Tüm servisler | Her servis kendi veritabanına sahip |

---

## 📡 API Endpoints

### Catalog Service (5011)

| Method | Endpoint | Açıklama |
|--------|----------|----------|
| GET | `/api/courses` | Tüm kursları listele |
| GET | `/api/courses/{id}` | ID ile kurs getir |
| GET | `/api/courses/GetAllByUserId/{userId}` | Kullanıcının kursları |
| POST | `/api/courses` | Yeni kurs oluştur |
| PUT | `/api/courses` | Kurs güncelle |
| DELETE | `/api/courses/{id}` | Kurs sil |
| GET | `/api/categories` | Tüm kategoriler |

### Basket Service (5013)

| Method | Endpoint | Açıklama |
|--------|----------|----------|
| GET | `/api/baskets` | Sepeti getir |
| POST | `/api/baskets` | Sepeti kaydet/güncelle |
| DELETE | `/api/baskets` | Sepeti sil |

### Order Service (5015)

| Method | Endpoint | Açıklama |
|--------|----------|----------|
| POST | `/api/orders` | Sipariş oluştur |
| GET | `/api/orders` | Kullanıcı siparişleri |

### PhotoStock Service (5012)

| Method | Endpoint | Açıklama |
|--------|----------|----------|
| POST | `/api/photos` | Fotoğraf yükle |
| DELETE | `/api/photos?photoUrl={url}` | Fotoğraf sil |

### Discount Service (5014)

| Method | Endpoint | Açıklama |
|--------|----------|----------|
| GET | `/api/discounts` | Kullanıcı indirimleri |
| GET | `/api/discounts/{code}` | Kod ile indirim getir |
| POST | `/api/discounts` | Yeni indirim oluştur |
| DELETE | `/api/discounts/{id}` | İndirim sil |

### Payment Service (5016)

| Method | Endpoint | Açıklama |
|--------|----------|----------|
| POST | `/api/fakepayments` | Ödeme al |

### IdentityServer (5001)

| Method | Endpoint | Açıklama |
|--------|----------|----------|
| POST | `/connect/token` | Token alma |
| GET | `/connect/userinfo` | Kullanıcı bilgileri |
| POST | `/api/user/signup` | Kullanıcı kaydı |

---

## 📝 Notlar

- **Üretim ortamı için:** `RequireHttpsMetadata = true` yapılmalı, güçlü secret'lar kullanılmalıdır
- **RabbitMQ Management UI:** http://localhost:15672 (guest/guest)
- **Swagger:** Her servis `/swagger` endpoint'inde API dokümantasyonuna sahiptir

---

## ⭐ Yıldız vermeyi unutmayın!

Projeyi beğendiyseniz GitHub'da ⭐ vermeyi unutmayın!