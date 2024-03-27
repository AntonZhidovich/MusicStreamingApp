import { Component, SkipSelf, importProvidersFrom } from '@angular/core';
import { Router, RouterOutlet } from '@angular/router';
import { SigninComponent } from '../signin/signin.component';
import { AuthService } from '../../services/auth.service';
import { HTTP_INTERCEPTORS, HttpClient, HttpClientModule } from '@angular/common/http';
import { AuthInterceptor } from '../../interceptors/auth.interceptor';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, SigninComponent, HttpClientModule],
  templateUrl: './app.component.html'
})
export class AppComponent {
  title = 'MusicStreamingAppClient';
  
  constructor(private authService: AuthService, private router: Router) {}
}
