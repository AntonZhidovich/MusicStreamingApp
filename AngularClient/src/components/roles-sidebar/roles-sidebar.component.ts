import { Component } from '@angular/core';
import { RoleModel } from '../../models/RoleModel';
import { RolesService } from '../../services/roles.service';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'roles-sidebar',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './roles-sidebar.component.html'
})
export class RolesSidebarComponent {
  roles: RoleModel[] = [];
  createRoleDetails = {
    isCreating: false,
    role: new RoleModel()
  }
  
  constructor(private roleService: RolesService) {}

  ngOnInit() {
    this.loadRoles();
  }

  onRemoveClick(role: RoleModel) {
    if (!confirm(`You sure you want to delete role ${role.name}?`)) {
      return;
    }

    this.roleService.deleteRole(role.name)
    .subscribe({
      next: (result) => {
        this.loadRoles();
      },
      error: (error) => {
        console.log(error);
      }});
  }

  onCreateClick() {
    this.createRoleDetails.isCreating = true;
  }
  
  onSaveClick() {
    this.roleService.createRole(this.createRoleDetails.role)
    .subscribe({
      next: (result) => {
        this.loadRoles();
        this.createRoleDetails.isCreating = false;
        this.createRoleDetails.role.name = "";
      },
    error: (error) => {
      console.log(error);
    }});
  }

  onCancelClick() {
    this.createRoleDetails.isCreating = false;
  }

  loadRoles() {
    this.roleService.getRoles()
      .subscribe({
        next: (result) => {
          this.roles = result;
        },
      error: (error) => {
        console.log(error);
      }});
  }
}
