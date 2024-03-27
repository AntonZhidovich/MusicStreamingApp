import { Component, effect } from "@angular/core";
import { SongModel } from "../../models/SongModel";
import { SongService } from "../../services/song.service";
import { PlayerService } from "../../services/player.service";

@Component({
    selector: "songs",
    standalone: true,
    templateUrl: "./songs.component.html"
})
export class SongsComponent {
    songs: SongModel[] = [];
    
    constructor(private songService: SongService,
                private playerService: PlayerService) {}

    ngOnInit() {
        this.songService.getSongs()
            .subscribe({
                next: (result) => {
                    this.songs = result.items;
                },
                error: (error) => {
                    console.log(error);
                }
            })
    }

    onSongClick(songNumber: number) {
        this.playerService.setPlaylist(this.songs, songNumber);
    }
}