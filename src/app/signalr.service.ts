import { publishFacade } from "@angular/compiler";
import { Injectable } from "@angular/core";
//import signalR from "@microsoft/signalr";
import * as signalR from "@microsoft/signalr";


@Injectable ({providedIn:'root'})
export class SignalrService{
    public hubConnection?:signalR.HubConnection;

   public startConnection(userId:string):void{
    if(this.hubConnection && this.hubConnection.state === signalR.HubConnectionState.Connected){
        return;
    }

    this.hubConnection=new signalR.HubConnectionBuilder().withUrl(`https://localhost:44329/chat-hub?userId=${userId}`,{
        skipNegotiation:true,
        transport:signalR.HttpTransportType.WebSockets
    })
    .withAutomaticReconnect()
    .build();

    this.hubConnection.start().then(()=>{
        console.log(`SignalR bağlnatısı başarılı.Kullanıcı ID:${userId}`);
    }).catch((err)=>{
        console.error('SignalR bağlantı hatası: ',err);

        setTimeout(()=>this.startConnection(userId),5000);
    });
    this.hubConnection.onclose((error)=>{
        console.warn('SignalR bağlnatısı kapandı.',error);
    });

   }

   public addMessageListener(methodName: string, callback: (...args: any[]) => void) {
    this.hubConnection?.on(methodName, callback);
  }

  public stopConnection(){
    this.hubConnection?.stop().then(()=>console.log("SignalR bağlantısı durduruldu. ")).catch(err=>console.error("Bağlantı durdurulurken hata:",err));
  }
}