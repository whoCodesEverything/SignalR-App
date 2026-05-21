
import { Component, OnInit } from '@angular/core';
import { Router, RouterOutlet } from '@angular/router';
import { AuthService } from './auth.service';
import { SignalrService } from './signalr.service';


@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit {
  user: any;
  constructor(private router: Router) { }



  ngOnInit(): void {
    this.checkUserAuthentication();
    // const token = localStorage.getItem("accessToken");
    // console.log("Gönderilen Token:", token); // Token gerçekten var mı?

    // if (!token || token === "[object Object]") {
    //     console.error("Token bulunamadı! Lütfen önce giriş yapın.");
    //     return;
    // }

    // this.authService.getCurrentUserFromApi().subscribe({
    //   next: (user) => {
    //     this.user = user;
    //     const id = user?.id;
    //     console.log(id);
    //     if (id) {
    //       this.signalrService.startConnection(id);
    //       console.log("SignalR bağlantısı başlatıldı. ID:", id);
    //     }
    //   },
    //   error: (err) => {
    //     console.error("API isteği sırasında hata oluştu. Token geçersiz olabilir:", err);
    //     // Eğer 401 hatası gelirse kullanıcıyı login'e yönlendirebilirsin
    //   }
    // });

  }

  //once login ol sonra getCurrentUserFromApi() fonksiyonu kontrolünü bak
  checkUserAuthentication() {
    const token = localStorage.getItem("accessToken");

    // Token yoksa veya hatalı kaydedilmişse
    if (!token || token === "[object Object]") {
          console.error("Token bulunamadı! Lütfen önce giriş yapın.");
        return;
     }
      // Eğer şu an zaten login sayfasındaysak tekrar yönlendirme yapma
      if (this.router.url !== '/login') {
        console.warn("Token bulunamadı, login sayfasına yönlendiriliyor...");
        this.router.navigate(['/login']);
      }
    }
  }


