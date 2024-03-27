import { Component, EventEmitter, HostListener, Output } from "@angular/core";
import { PlaylistModel } from "../../models/PlaylistModel";
import { PlaylistService } from "../../services/playlist.service";
import { FormsModule } from "@angular/forms";
import { PlayerService } from "../../services/player.service";
import { SongModel } from "../../models/SongModel";

@Component({
    selector: "song-queue",
    standalone: true,
    templateUrl: "./song-queue.component.html",
    imports: [FormsModule],
    providers: [PlaylistService]
})
export class SongQueueComponent {

    @Output() onAddToPlaylist = new EventEmitter<SongModel>();

    height: number = 0;
    width:number = 450;
    playlists: PlaylistModel[] = [];
    
    songs: SongModel[] = [];
    currentIndex = 0;

    resizeInfo = {
        isPressed: false,
        minWidth: 450,
        maxWidth: window.outerWidth*0.6
    }

    constructor(public playerService: PlayerService,
                private PlaylistService: PlayerService) {}

    onSongSelected(index: number) {
        this.playerService.setCurrentSong(index);
    }

    onRemoveClick(index: number, event: Event){
        event.stopPropagation();
        this.playerService.removeSong(index);
    }

    onAddClick(song: SongModel, event: Event){
        event.stopPropagation();
        this.onAddToPlaylist.emit(song);
    }

    OnResizerMouseDown(event: MouseEvent) {
        this.resizeInfo.isPressed = true;
    }


    @HostListener("window:mousemove", ["$event"])
    OnMouseMove(event: MouseEvent){
        if (!this.resizeInfo.isPressed){
            return;
        }

        let mouseX = event.clientX;

        if (mouseX >= this.resizeInfo.minWidth && mouseX <= this.resizeInfo.maxWidth){
            this.width = mouseX;
        }
    }

    @HostListener("window:mouseup", ["$event"])
    OnMouseUp(event: MouseEvent){
        this.resizeInfo.isPressed = false;
    }
}