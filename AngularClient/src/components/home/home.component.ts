import { Component } from "@angular/core";
import { SidebarComponent } from "../sidebar/sidebar.component";
import { RouterOutlet } from "@angular/router";
import { NavbarComponent } from "../navbar/navbar.component";
import { SongQueueComponent } from "../song-queue/song-queue.component";
import { AddToPlaylistModalComponent } from "../add-to-playlist-modal/add-to-playlist-modal.component";
import { SongModel } from "../../models/SongModel";

@Component({
    selector: "main",
    standalone:true,
    templateUrl: "./home.component.html",
    imports: [SidebarComponent, SongQueueComponent, AddToPlaylistModalComponent, NavbarComponent, RouterOutlet]
})
export class HomeComponent {

    addSongToPlaylistDetails = {
        showModal: false,
        song: new SongModel()
    }

    constructor() {}

    addSongToPlaylist(song: SongModel) {
        this.addSongToPlaylistDetails.song = song;
        this.addSongToPlaylistDetails.showModal = true;
    }
}