# Vertigo Games – Wheel of Fortune Case Study (Unity / C#)

Bu repo, Vertigo Games Game Developer Demo dokümanındaki gereksinimlere göre hazırlanmış “wheel of fortune” temalı bir mini oyun demosudur.  
Oyuncu her zoneda çarkı çevirir ödül kazanır veya “bomba” gelirse o ana kadar biriken ödüller sıfırlanır, devam edilebilir, oyun yeniden başlatılabilir.
Her zone bölgesi ilrledikçe ödül çarpanları artmaktadır.

## Özellikler (Kısa)
- Zone progression:
  - Normal zone: bomba + standart ödül havuzu
  - Safe zone: bomba yok
  - Super zone: bomba yok + özel ödül havuzu
- Spin akışı: Spin -> sonuç -> ödül/bomba -> Kart -> Envanter -> Yeni Level
- Ödül envanteri + kart animasyonu + partikül efekt
- UI kuralları: TextMeshPro kullanımı, bazı otomatik editor setupları (OnValidate) ve buton listenerlarının koddan bağlanması

---

## Gereksinimler
Bu proje/sistemler aşağıdaki paketleri kullanıyor:
- **TextMeshPro** 
- **DOTween** 
- **Zenject**
- **UniTask** 


## Nasıl Oynanır
- **SPIN** butonuna bas.
- Sonuç:
  - Ödül geldiyse: kart/partikül animasyonu + envantere eklenir
  - Bomb geldiyse: bomba panelş açılır, devam edebilir veya çıkabilirsiniz
- Zone ilerledikçe ödül sayısı artar.
- Super zone bölümüne gelince özel ödül gelir, standart ödüllerin değeri katlanır

---

## Proje Yapısı (Özet)
`Development/Scripts` altında klasörler:

- **Core/**
  - ScriptableObject data’lar (SpinSettingsDataSO, GameDataSO, ZoneData vb.)
  - EventBus ve event struct’ları
- **Managers/**
  - GameManager (bootstrapping + initializer)
- **Systems/**
  - WheelSystem (spin + wheel visual + slice populate)
  - ZoneSystem (level/zone tip yönetimi)
  - InventorySystem (ödül listesi ve item UI)
  - CardSystem (spin sonucu kart animasyonu)
  - PanelSystem (bomb/exit/start panelleri)
  - InfoSystem (top/left UI info panelleri)
- **Helpers/**
  - Extensions, Zenject installer, küçük yardımcı sınıflar

---

## Mimari Notları
- Sistemler arası iletişim için **Event Bus** kullanıldı (Publish/Subscribe).
- Görsel/davranış konfigürasyonu **ScriptableObject** ile data-driven ilerliyor.
- Wheel dönme matematiği ve wheel içeriğini doldurma işleri “service” sınıflarına ayrıldı (WheelSpinnerService / WheelInventoryService).
- Bazı referanslar Zenject ile **Inject** ediliyor; sahnede SceneContext/Installer setup’ı bu yüzden önemli.
- Spin animasyonu designer edasıyla editörden spinsettings datası üzerinden değiştirilebilir
- Oyuncunun kazanbileceği itemlar editörden özel editör koduyla değiştirilebilir, yenisi eklenebilir ve boyutlandırma ayarları yapılabilir.
- SOLID prensipleri, design patternlara olabildiğince uyum sağlanmya çalışılmıştır.
- 9-Sliced Texture ve SpriteAtlas kullanılmıştır.

---

## Bilinen Kısıtlar / Notlar
- Bu içerik bir case demo olduğu için bazı alanlar hızlı prototip mantığında tutulmuştur.
- Reset sistemi yazılmadı ve bazı yerlerde küçük eksiklikler oalbilir
- Hata testi yeterince yapılmamıştır. Demo seviyesinde tutulmuştur.

---

## Not
Case kapsamında istenen kurallara uyum (UI, event kullanımı, data-driven yaklaşım) **özellikle** hedeflenmiştir.
