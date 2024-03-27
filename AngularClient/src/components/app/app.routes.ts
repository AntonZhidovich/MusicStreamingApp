import { Routes } from '@angular/router';
import { SigninComponent } from '../signin/signin.component';
import { MainComponent } from '../main/main.component';
import { RegistrerComponent } from '../register/register.component';
import { AuthorsComponent } from '../authors/authors.component';
import { CreatorComponent } from '../creator/creator.component';
import { ReleasesComponent } from '../releases/releases.component';
import { SongsComponent } from '../songs/songs.component';
import { HomeComponent } from '../home/home.component';
import { PlaylistsComponent } from '../playlists/playlists.component';


export const homeRoutes: Routes = [
    {path: "authors", component: AuthorsComponent},
    {path: "releases", component: ReleasesComponent},
    {path: "songs", component: SongsComponent},
    {path: "playlists", component: PlaylistsComponent}
]

export const mainRoutes: Routes = [
    {path: "home", component: HomeComponent, children: homeRoutes},
    {path: "creator", component: CreatorComponent},
];

export const routes: Routes = [
    {path: "", component: MainComponent, children: mainRoutes},
    {path: "authorization", component: SigninComponent},
    {path: "registration", component: RegistrerComponent},
    {path: "**", redirectTo: "home/songs"}
];
