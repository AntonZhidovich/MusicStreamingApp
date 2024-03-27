import { Component, Input } from "@angular/core";
import { AuthService } from "../../services/auth.service";
import { Router, RouterLink } from "@angular/router";
import { PlayerComponent } from "../player/player.component";
import { SongModel } from "../../models/SongModel";

@Component({
    selector: "navbar",
    standalone: true,
    templateUrl: "./navbar.component.html",
    imports: [RouterLink, PlayerComponent]
})
export class NavbarComponent {

    showDropDown = false;
    showPlayer = false;

    @Input() songToPlay = new SongModel();

    constructor(private authService: AuthService,
                private router: Router) {}

    onLogoutClick() {
        this.authService.logOut();
        this.router.navigate(["/"]);
    }

    onShowDropdown() {
        this.showDropDown = !this.showDropDown;
    }
}