import { Component, EventEmitter, Input, Output } from '@angular/core';
import { SongModel } from '../../models/SongModel';

@Component({
  selector: 'song-detail',
  standalone: true,
  imports: [],
  templateUrl: './song-detail.component.html'
})
export class SongDetailComponent {
  @Input() editMode = true;
  @Input() song: SongModel = new SongModel();
  @Output() onDelete = new EventEmitter<SongModel>()

  onDeleteButton() {
    this.onDelete.emit(this.song);
  }
}
