import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable, catchError, throwError } from "rxjs";
import { AuthService } from "../services/auth.service";
import { Router } from "@angular/router";
import { Tokens } from "../models/Tokens";

@Injectable({
    providedIn: "root"
})
export class AuthInterceptor implements HttpInterceptor {

    private returnUrl: string = "";
    constructor(private authService: AuthService,
                private router: Router) {}

    intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        this.returnUrl = this.router.lastSuccessfulNavigation?.initialUrl.toString()!;
        var accessToken = this.authService.getAccessToken();

        if (accessToken) {
            request = this.setAuthorization(request, accessToken);
        }
        
        return next.handle(request).pipe(catchError((error) => {
            if (!this.authService.isRefreshingToken){
                if (error.status == 401 || error.status == 403) {
                    
                    if (accessToken) {
                        return this.useRefreshToken(request, next, accessToken);
                    } else {
                        this.redirectToAuthorization();
                    }
                }  
            }
                   
            return throwError(() => error);
        }));
    }

    private useRefreshToken(request: HttpRequest<any>, next: HttpHandler, accessToken: string) : Observable<HttpEvent<any>> {
        this.authService.useRefreshToken()
            .subscribe({
                next: (tokens: Tokens) => {
                    this.authService.storeTokens(tokens);
                    request = this.setAuthorization(request, accessToken);
                    return next.handle(request);
                },
                error: (error) => {
                    this.authService.logOut();
                    console.log(error);
                    this.redirectToAuthorization();
                }
            })

            return throwError(() => new Error("Unknown authorization error"));
    }

    private setAuthorization(request: HttpRequest<any>, accessToken: string) : HttpRequest<any> {
        request = request.clone({
            setHeaders: {
                Authorization: `Bearer ${accessToken}`
            }
        })

        return request;
    }

    private redirectToAuthorization(){
        this.router.navigate(["/authorization"], {queryParams: {returnUrl : this.returnUrl }});
    }
}