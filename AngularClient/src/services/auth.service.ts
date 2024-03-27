import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { LoginModel } from '../models/LoginModel';
import { environment } from '../environments/environment';
import { Tokens } from '../models/Tokens';
import { RegisterModel } from '../models/RegisterModel';
import { Observable, firstValueFrom, tap } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  public loggedIn = false;
  private authUrl = "identity/authorization";
  private registerUrl = "identity/users";

  isRefreshingToken = false;

  constructor(private http: HttpClient) { }

  async login(model: LoginModel) : Promise<boolean> {
    let path = `${environment.gatewayUrl}/${this.authUrl}/`;
      
    var tokens = await firstValueFrom(this.http.post<Tokens>(path, model)).catch(() => {});
    
    if (tokens) {
      this.storeTokens(tokens);
      this.loggedIn = true;
    }
    else{
      this.loggedIn = false;
    }

    return this.loggedIn;
  }

  logOut(){
    localStorage.clear();
  }

  getAccessToken() : string | null {
    return localStorage.getItem("access_token");
  }

  getRefreshToken() : string | null {
    return localStorage.getItem("refresh_token");
  }

  register(model: RegisterModel){
    let path = `${environment.gatewayUrl}/${this.registerUrl}`;
    return this.http.post(path, model);
  }

  getAuthorizedUsername() : string {
    let accessToken = this.getAccessToken();
    let username = "";

    if(accessToken) {
      let decodedJWT = JSON.parse(window.atob(accessToken.split('.')[1]));
      username =  decodedJWT.unique_name;
    }

    return username;
  }

  useRefreshToken() : Observable<Tokens> {
    let accessToken = this.getAccessToken();
    let refreshToken = this.getRefreshToken();
    let path = `${environment.gatewayUrl}/${this.authUrl}/refresh`;
    
    let model = new Tokens(accessToken!, refreshToken!);
    this.isRefreshingToken = true;
    return this.http.post<Tokens>(path, model)
      .pipe(tap(() => this.isRefreshingToken = false));
  }

  storeTokens(tokens: Tokens) {
    localStorage.setItem("access_token", tokens.accessToken);
    localStorage.setItem("refresh_token", tokens.refreshToken);
  }
}
