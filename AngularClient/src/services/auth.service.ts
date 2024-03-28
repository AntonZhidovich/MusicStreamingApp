import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { LoginModel } from '../models/LoginModel';
import { Tokens } from '../models/Tokens';
import { RegisterModel } from '../models/RegisterModel';
import { Observable, firstValueFrom, tap } from 'rxjs';
import { endpoints } from '../endpoints';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  public loggedIn = false;

  isRefreshingToken = false;

  constructor(private http: HttpClient,
              private router: Router) { }

  async login(model: LoginModel) : Promise<boolean> {
      
    var tokens = await firstValueFrom(this.http.post<Tokens>(endpoints.authorization, model)).catch(() => {});
    
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
    return this.http.post(endpoints.registration, model);
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
    
    let model = new Tokens(accessToken!, refreshToken!);
    this.isRefreshingToken = true;
    return this.http.post<Tokens>(endpoints.refresh, model)
      .pipe(tap(() => this.isRefreshingToken = false));
  }

  storeTokens(tokens: Tokens) {
    localStorage.setItem("access_token", tokens.accessToken);
    localStorage.setItem("refresh_token", tokens.refreshToken);
  }
}
