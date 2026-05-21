import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
//import { HttpClient } from '@microsoft/signalr';
import { HttpClient, HttpHeaders } from '@angular/common/http'
import { catchError, Observable, of, tap } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = 'https://localhost:44329/api/Auth/GetCurrentUser'; // API adresinizi buraya yazın
  constructor(private router: Router, private http: HttpClient) { }

  isAuthenticated(): boolean {
    console.log('test');
    const token = localStorage.getItem("accessToken");
    if (token && token.split('.').length === 3) {
      return true;
    }
    
    this.router.navigateByUrl("/login");
    return false;
  }

  getCurrentUserFromApi(): Observable<any> {
    const token = localStorage.getItem("accessToken");
    if (!token || token.split('.').length < 3) return of(null);

    // Header'a Token'ı ekliyoruz
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}.trim()}`
    });

    return this.http.get(this.apiUrl, { headers }).pipe(
      tap(user=>{
        if(user) console.log("Kullanıcı başarıyla alındı.",user);
      }),
     catchError(err => {
      if(err.status===401){
        console.error("Token gecersiz veya süresi dolmuş");
      }
      return of(null);
     })
    );
  }

  // getCurrentUser() {
  //   try {
  //     const token = localStorage.getItem("accessToken");
  //     if (!token || token.split('.').length < 3) return null;


  //     //token ile api/auth/getCurrentUser servisine istek atılır. UserId oradan okunur.


  //     // Türkçe karakter desteği için decodeURIComponent ve atob kombinasyonu
  //     const base64 = token.split('.')[1].replace(/-/g, '+').replace(/_/g, '/');
  //     const jsonPayload = decodeURIComponent(atob(base64).split('').map(function (c) {
  //       return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
  //     }).join(''));

  //     const payload = JSON.parse(jsonPayload);

  //     return {
  //       // Paylaştığın token'daki tam karşılığı:
  //       id: payload["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"],
  //       name: payload["userName"] || payload["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"]
  //     };
  //   } catch (e) {
  //     console.error("Giriş yapan kullanıcı bilgileri okunamadı:", e);
  //     return null;
  //   }
  // }
}