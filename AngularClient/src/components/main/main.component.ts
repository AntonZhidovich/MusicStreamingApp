import { Component } from "@angular/core";
import { SidebarComponent } from "../sidebar/sidebar.component";
import { Router, RouterOutlet } from "@angular/router";
import { NavbarComponent } from "../navbar/navbar.component";
import { SongModel } from "../../models/SongModel";

@Component({
    selector: "main",
    standalone:true,
    templateUrl: "./main.component.html",
    imports: [SidebarComponent, NavbarComponent, RouterOutlet]
})
export class MainComponent {
    constructor() {}
}