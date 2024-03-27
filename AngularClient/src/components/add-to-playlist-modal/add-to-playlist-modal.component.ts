import { Component, EventEmitter, Input, Output } from "@angular/core";
import { PlaylistService } from "../../services/playlist.service";
import { SongModel } from "../../models/SongModel";
import { PlaylistModel } from "../../models/PlaylistModel";
import { FormsModule } from "@angular/forms";
import { DatePipe } from "@angular/common";
import { SongDetailComponent } from "../song-detail/song-detail.component";

@Component({
    selector: "add-to-playlist-modal",
    standalone: true,
    templateUrl: "./add-to-playlist-modal.component.html",
    providers: [],
    imports: [SongDetailComponent, FormsModule, DatePipe]
})
export class AddToPlaylistModalComponent {
    @Output() showModalChange = new EventEmitter<boolean>();
    @Input() song: SongModel = new SongModel();
    playlists: PlaylistModel[] = [];
    selectedPlaylistId = "";

    _showModal = false;

    @Input() set showModal(value: boolean){
      this._showModal = value;
      
      if (this._showModal) {
          this.loadPlaylists();
      }
    }

    constructor(private playlistService: PlaylistService) {}

    onSave() {
      this.playlistService.addSongToPlaylist(this.selectedPlaylistId, this.song.id)
        .subscribe({
          next: () => {
            this.onCancel();
          },
          error: (error) => {
            console.log(error);
          }
        });
    }

    onCancel() {
        this.showModal = false;
        this.showModalChange.emit(false);
    }

    loadPlaylists() {
        this.playlistService.getPlaylists()
          .subscribe(({
            next: (result) => {
              this.playlists = result.reverse();
            },
            error: (error) => {
              console.log(error);
            }
          }));
      }
}