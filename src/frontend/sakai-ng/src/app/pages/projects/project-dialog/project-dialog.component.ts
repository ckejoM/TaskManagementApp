import { NgIf } from '@angular/common';
import { Component, input, OnInit, output } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ButtonModule } from 'primeng/button';
import { DialogModule } from 'primeng/dialog';
import { CreateProjectRequest, ICreateProjectRequest, IUpdateProjectRequest, ProjectResponse, UpdateProjectRequest } from '../../../shared/apiClient';

@Component({
  selector: 'app-project-dialog',
  imports: [FormsModule, NgIf, DialogModule, ButtonModule],
  templateUrl: './project-dialog.component.html',
  styleUrl: './project-dialog.component.scss'
})
export class ProjectDialogComponent implements OnInit {
  projectToEdit = input<ProjectResponse>();
  update = output<UpdateProjectRequest>();
  create = output<CreateProjectRequest>();
  cancel = output();
  buttonLabel = "Create Project";
  projectName = "";
  projectDescription = "";
  
  ngOnInit(): void {
    if(this.projectToEdit != null && this.projectToEdit()?.id !== null && this.projectToEdit()?.id !== undefined){
      this.buttonLabel = "Update Project";
      this.projectName = this.projectToEdit()?.name!;
      this.projectDescription = this.projectToEdit()?.description!;
    } 
  }  

  cancelClick(){
    this.cancel.emit();
  }

  saveClick(){
    if(this.projectToEdit != null){
      let updateProjectRequest: IUpdateProjectRequest = {};
      updateProjectRequest.name = this.projectName;
      updateProjectRequest.description = this.projectDescription;
      updateProjectRequest.rowVersion = this.projectToEdit()?.rowVersion!;
      this.update.emit(new UpdateProjectRequest(updateProjectRequest));
    } else {
      let createProjectRequest: ICreateProjectRequest = {};
      createProjectRequest.name = this.projectName;
      createProjectRequest.description = this.projectDescription;
      this.create.emit(new CreateProjectRequest(createProjectRequest));
    }
  }

}
