import { Component } from '@angular/core';
import { PlaylistService } from '../../services/playlist.service';
import { PlaylistModel } from '../../models/PlaylistModel';
import { FormsModule } from '@angular/forms';
import { EditPlaylistComponent } from '../edit-playlist-modal/edit-playlist-modal.component';
import { PlaylistWithSongsModel } from '../../models/PlaylisWithSongsModel';
import { PlayerService } from '../../services/player.service';

@Component({
  selector: 'app-playlists',
  standalone: true,
  providers: [PlaylistService],
  imports: [EditPlaylistComponent, FormsModule],
  templateUrl: './playlists.component.html'
})
export class PlaylistsComponent {

  playlists: PlaylistModel[] = [];
  errorMessage: string | undefined;
  editPlaylistDetail = {
    showModal: false,
    playlistId: ""
  };

  newPlaylistInfo = {
    creatingMode: false,
    name: ""
}

  constructor(private playlistService: PlaylistService,
              private playerService: PlayerService) {}

  ngOnInit() {
    this.loadPlaylists();
  }

  onNewPlaylistClick(){
    this.newPlaylistInfo.creatingMode = true;
  }

  onCancelClick(){
    this.errorMessage = undefined;
    this.newPlaylistInfo.creatingMode = false;
    this.newPlaylistInfo.name = "";
  }

  onSaveClick(){
    this.errorMessage = undefined;
    this.playlistService.create(this.newPlaylistInfo.name)
    .subscribe({
        next: () => {
            this.newPlaylistInfo.name = "";
            this.newPlaylistInfo.creatingMode = false;
            this.loadPlaylists();
        },
        error: (error) => {
            console.log(error);
            this.errorMessage = error.error.title;
        }
    });

    this.loadPlaylists();
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

  onEditClick(playlist: PlaylistModel, event: Event) {
    event.stopPropagation();
    this.editPlaylistDetail.playlistId = playlist.id;
    this.editPlaylistDetail.showModal = true;
  }

  onPlaylistClick(playlist: PlaylistModel) { 
    this.playlistService.getPlaylistWithSongs(playlist.id)
      .subscribe({
        next: (result) => {
          this.addPlaylistToPlayer(result);
        },
        error: (error) => {
          console.log(error);
        }
      });
    
  }

  addPlaylistToPlayer(playlsit: PlaylistWithSongsModel) {
    this.playerService.setPlaylist(playlsit.songs);
  }
 
  onDeleteClick(id: string, name: string, event: Event){
    event.stopPropagation();
        
    if (!confirm(`You sure you want to delete playlist ${name}?`)){
        return;
    }

    this.playlistService.delete(id)
    .subscribe({
        next: () => {this.loadPlaylists();},
        error: (error) => {console.log(error)}
    });
  }
}
