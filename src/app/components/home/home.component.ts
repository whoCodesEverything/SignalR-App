import { CommonModule } from '@angular/common';
import { Component, OnInit, signal } from '@angular/core';
import { UserModel } from '../../models/user.model';
import { ChatModel } from '../../models/chat.model';
import { HttpClient } from '@angular/common/http';
import * as signalR from '@microsoft/signalr';
import { FormsModule } from '@angular/forms';
import { AuthService } from '../../auth.service';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule,FormsModule],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent implements OnInit{
  users: UserModel[] = [];
  chats: ChatModel[] = [];
  selectedUserId: string = "";
  selectedUser: UserModel = new UserModel();
  //user = new UserModel();
  user:UserModel|null|any;
  hub: signalR.HubConnection | undefined;
  message: string = "";  

  constructor(
    private authService:AuthService,
    private http: HttpClient
  ){
    //this.user = JSON.parse(localStorage.getItem("accessToken") ?? "");
    console.log(localStorage.getItem("accessToken"));

    const token = localStorage.getItem("accessToken");

      if (token) {
        const base64Url = token.split('.')[1]; // Orta parçayı al
        const base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
        const jsonPayload = decodeURIComponent(atob(base64).split('').map(function(c) {
            return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
        }).join(''));

        this.user = JSON.parse(jsonPayload); // jsonPayload artık geçerli bir JSON string'idir.
      }

    this.getUsers();

    this.hub = new signalR.HubConnectionBuilder().withUrl("https://localhost:44329/chat-hub").build();
    
    if(this.user.id && this.user.id!=='undefined'){
      this.hub.start();
    }
    
    this.hub.start().then(()=> {
      console.log("Connection is started...");  
      
      this.hub?.invoke("Connect", this.user.id);
      // console.log(this.user.id);

      this.hub?.on("Users", (res:UserModel) => {
        console.log(res);
        this.users.find(p=> p.id == res.id)!.status = res.status;        
      });

      this.hub?.on("Messages",(res:ChatModel)=> {
        console.log(res);        
        
        if(this.selectedUserId == res.userId){
          this.chats.push(res);
        }
      })
    })
  }

  ngOnInit(){
    this.getUsers();

  }
  getUsers(){
    this.http.get<UserModel[]>("https://localhost:44329/api/Auth/GetUser").subscribe(res=> {
      this.users = res.filter(p => p.id != this.user.id);
    })
  }

  // changeUser(user: UserModel){
  //   this.selectedUserId = user.id;
  //   this.selectedUser = user;

  //  // const currentUserId=this.user?.id || this.user?.userId;

  //   if(!this.user?.id || this.user?.id){
  //     console.error("hata:kendi kullanıcı bilgileriniz (this.user) bulunamdı!");
  //     return;
  //   }

  //   console.log("Giriş yapan kullanıcı:",this.user);
  //   console.log("Seçilen kullanıcı ID:",this.selectedUserId);
  //   this.http.get(`https://localhost:44329/api/Chat/GetChats?userId=${this.user.id}&toUserId=${this.selectedUserId}`).subscribe((res:any)=>{
  //     this.chats = res;
  //   });
  // }


changeUser(user: UserModel) {
 const currentUserData=this.authService.getCurrentUserFromApi();
 if(!currentUserData){
  console.error("Kullanıcı oturumu bulunmadı!");
  return;
 }
 this.user = currentUserData;

  this.selectedUserId = user.id;
  this.selectedUser = user;

 
  const currentUserId = this.user?.id || this.user?.id;

  // 2. Doğru mantıksal kontrol: EĞER ID YOKSA DUR
  if (!currentUserId) {
    console.error("Hata: Kendi kullanıcı ID'niz bulunamadı! 'this.user' nesnesini kontrol edin.");
    return; // Burada durması gerekir
  }

  console.log("İstek gönderiliyor... Gönderen:", currentUserId, "Alıcı:", this.selectedUserId);

  // 3. API İsteğinde 'this.user.id' yerine yukarıdaki 'currentUserId' değişkenini kullanın
  this.http.get(`https://localhost:44329/api/Chat/GetChats?userId=${currentUserId}&toUserId=${this.selectedUserId}`)
    .subscribe({
      next: (res: any) => {
        this.chats = res;
      },
      error: (err) => {
        console.error("API Hatası:", err);
      }
    });
}
  logout(){
    localStorage.clear();
    document.location.reload();
  }

  sendMessage(){
    const data ={
      "userId": this.user.id,
      "toUserId": this.selectedUserId,
      "message": this.message
    }
    this.http.post<ChatModel>("https://localhost:44329/api/Chats/SendMessage",data).subscribe(
      (res)=> {
        this.chats.push(res);
        this.message = "";
    });
  }

}



