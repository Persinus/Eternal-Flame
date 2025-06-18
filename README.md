# Eternal Flame

Eternal Flame là một game nhập vai hành động offline, lấy cảm hứng từ các tựa game nổi tiếng như Ninja School hay Ngôc Rồng Online của Teamobi. Trong game, bạn sẽ hóa thân vào nhân vật chính, phiêu lưu qua nhiều bản đồ khác nhau, đối đầu quái vật và thu thập trang bị để nâng cao sức mạnh.

---

## **Tính Năng Chính**
- **Chiến đấu offline:** Không cần kết nối internet.
- **Hệ thống nhân vật phong phú:** Mỗi nhân vật có bộ kỹ năng và phong cách chiến đấu độc đáo.
- **Cây kỹ năng:** Học và nâng cấp kỹ năng cho nhân vật.
- **Trang bị đa dạng:** Thu thập vũ khí, giáp và phụ kiện.

---

## **Cài Đặt Docker**

Eternal Flame sử dụng Docker để đồng bộ môi trường phát triển và build dự án. Hướng dẫn dưới giúp bạn cài đặt và build game.

### **1. Cài Đặt Docker**
- **Windows/MacOS:** Tải Docker Desktop tại [Docker Desktop](https://www.docker.com/products/docker-desktop/).
- **Linux:** Cài đặt Docker Engine:
  ```bash
  sudo apt-get update
  sudo apt-get install -y docker.io
  sudo systemctl start docker
  sudo systemctl enable docker
  ```

### **2. Build Docker Image**
Chạy lệnh sau trong thư mục chứa Dockerfile:
```bash
docker build -t eternal-flame-unity:alpha .
```

### **3. Chạy Container**
Sử dụng Docker container để build Unity project:
```bash
docker run -it -v $(pwd):/project eternal-flame-unity:alpha \
/opt/Unity/Editor/Unity -batchmode -nographics -projectPath /project -buildTarget StandaloneWindows64 -quit
```

### **4. Xuất Bản Build Artifact**
Build artifact sẽ được tải lên GitHub Actions hoặc nếu chạy local, bạn có thể tìm thấy trong thư mục `Build/`.

---

### **Cấu Hình Unity**
Để đảm bảo Unity hoạt động đúng trong Docker, bạn cần cấu hình một số tham số:
- **Unity Version:** Sử dụng Unity 2020.3.30f1 hoặc phiên bản tương thích.
- **Build Target:** Chọn `StandaloneWindows64` cho Windows hoặc `StandaloneLinux64` cho Linux.
- **Project Path:** Đảm bảo đường dẫn đến project Unity là chính xác.
- **Build Path:** Chỉ định thư mục xuất build, ví dụ: `/project/Build/`.

### **5. Clone Game**
```bash
git clone https://github.com/Persinus/Eternal-Flame.git
cd Eternal-Flame
```
## **Hướng Dẫn Chơi Game**
1. Tải file cài đặt (Build artifact) từ GitHub Actions hoặc nhóm phát triển.
2. Chạy file cài đặt và bắt đầu phiêu lưu trong Eternal Flame.

---

## **Liên Hệ**
- Website: [eternalflame-game.com](https://eternalflame-game.com)
- Email hỗ trợ: nguyenmanh2004devgame@gmail.com

Hãy tham gia và cùng khám phá thế giới Eternal Flame ngay hôm nay!