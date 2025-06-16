import { Component, OnInit } from '@angular/core';
import { CreateProjectRequest, ProjectResponse, ProjectService, UpdateProjectRequest } from '../../../shared/apiClient';
import { TableModule } from 'primeng/table';
import { CommonModule } from '@angular/common';
import { TagModule } from 'primeng/tag';
import { ButtonModule } from 'primeng/button';
import { Dialog } from 'primeng/dialog';
import { FormsModule } from '@angular/forms';
import { ToastService } from '../../../shared/services/toast.service';
import { ProjectDialogComponent } from '../project-dialog/project-dialog.component';

@Component({
  selector: 'app-project-list',
  imports: [TableModule, TagModule, ButtonModule, CommonModule, FormsModule, ProjectDialogComponent],
  templateUrl: './project-list.component.html',
  styleUrl: './project-list.component.scss'
})
export class ProjectListComponent implements OnInit {
  projectToEdit!: ProjectResponse;
  projectToCreate: CreateProjectRequest = new CreateProjectRequest();
  displayDialog = false;

constructor(private projectService: ProjectService, private toastService: ToastService) {}

projects: ProjectResponse[] = [];

  ngOnInit(): void {
    this.getProjects();
  }

  getProjects(){
    this.projectService.getAll().subscribe(res => {
          console.log(res);
          this.projects = res;
        });
  }

  deleteProject(project: ProjectResponse){
    this.projectService.delete(project.id!).subscribe(() => {
      this.toastService.showSuccess('Project deleted');
      this.projects = this.projects.filter(p => p.id !== project.id);
    });
  }

  editProject(project: ProjectResponse){
    this.projectToEdit = JSON.parse(JSON.stringify(project)) as ProjectResponse; // deep copy of project;
    this.displayDialog = true;
  }

  openDialog(){
    this.displayDialog = true;
  }

  closeDialog(){
    this.projectToEdit = new ProjectResponse();
    this.displayDialog = false;
  }

  updateProject(project: UpdateProjectRequest){
    this.projectService.update(this.projectToEdit.id!, project).subscribe(() => {
      this.toastService.showSuccess('Project updated');
      this.projectToEdit = new ProjectResponse();
      this.displayDialog = false;
      this.getProjects();
    });
  }

  createProject(project: CreateProjectRequest){
    this.projectService.create(project).subscribe(() => {
      this.displayDialog = false;
      this.toastService.showSuccess('Project created');
      this.getProjects();
    });
  }

}
