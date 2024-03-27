import { Component, EventEmitter, Input, Output } from '@angular/core';
import { PlaylistService } from '../../services/playlist.service';
import { PlaylistWithSongsModel } from '../../models/PlaylisWithSongsModel';
import { SongDetailComponent } from '../song-detail/song-detail.component';
import { SongModel } from '../../models/SongModel';

@Component({
  selector: 'edit-playlist-modal',
  standalone: true,
  imports: [SongDetailComponent],
  providers: [PlaylistService],
  templateUrl: './edit-playlist-modal.component.html'
})
export class EditPlaylistComponent {
  @Input() playlistId = "";
  
  @Input() set showModal(value: boolean){
    console.log("here");
    this._showModal = value;
    if (this._showModal) {
      this.loadPlaylistInfo();
    }
  }
  
  @Output() showModalChange = new EventEmitter<boolean>();

  _showModal = false;
  playlist = new PlaylistWithSongsModel();

  constructor(private playlistService: PlaylistService) {}

  onCancel() {
    this.showModal = false;
    this.showModalChange.emit(false);
} 

onDeleteSong(song: SongModel) {
  if (!confirm(`You sure you want to delete ${song.title} song from playlist ${this.playlist.name}?`)) {
    return;
  }

  this.playlistService.deleteSongFromPlaylist(this.playlist.id, song.id)
    .subscribe({
      next: (result) => {
        this.loadPlaylistInfo();
      },
      error: (error) => {
        console.log(error);
      }
    });
}

loadPlaylistInfo() {
  this.playlistService.getPlaylistWithSongs(this.playlistId)
    .subscribe({
      next: (result) => {
        this.playlist = result;
      },
      error: (error) => {
        console.log(error);
      }});
}

}
