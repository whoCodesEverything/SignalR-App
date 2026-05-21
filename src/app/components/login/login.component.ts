import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../auth.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
name: string = "";

constructor(
  private http: HttpClient,
  private authService:AuthService,
  private router: Router
){

}
login() {
    const loginData = { name: this.name }; 
    
   // console.log("İstek atılıyor: https://localhost:44329/api/Auth/Login", loginData);

    this.http.post("https://localhost:44329/api/Auth/Login", loginData).subscribe({
      next: (res:any) => {
        console.log("Sunucudan gelen yanıt:", res);
      // const token = (typeof res === 'object') ? (res.token || res.accessToken) : res;

      const token = res?.token || res?.accessToken || (typeof res==='string'?res:null)

       if (token) { 
        
        localStorage.setItem("accessToken",token.trim());
        ///kullanıcı bilgilerini çekme
        this.authService.getCurrentUserFromApi().subscribe({next:(user)=>{
          if(user){
            console.log("Giriş başarılı, yönlendiriliyor..",user);
            this.router.navigate(['/']);
          }else{
            alert("Giriş yapıldı ama kullanıcı bilgilerii alınamadı.");
          }
        },
        error:(err)=>{
          console.log("Kullanıcı detayı çekilirken hata",err);
        }
      }
      );  
        this.router.navigate(["/"]);}     
      },
      error: (err) => {
        console.error("Giriş hatası detayları:", err);
        
        if (err.status === 405) {
          console.error("HATA 405: Sunucu POST isteğini reddetti. Lütfen Backend Controller'da [HttpPost] olduğundan emin olun.");
        } else if (err.status === 0) {
          console.error("HATA: Sunucuya ulaşılamıyor. CORS hatası olabilir veya backend çalışmıyor.");
        }
        
        alert("Giriş sırasında bir hata oluştu. Konsol çıktılarını kontrol edin.");
      }
    });
  }
}