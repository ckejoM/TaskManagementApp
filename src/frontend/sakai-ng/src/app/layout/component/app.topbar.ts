import { Component, inject } from '@angular/core';
import { MenuItem, MessageService } from 'primeng/api';
import { RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { StyleClassModule } from 'primeng/styleclass';
import { AppConfigurator } from './app.configurator';
import { LayoutService } from '../service/layout.service';
import { ToastService } from '../../shared/services/toast.service';
import { DropdownModule } from 'primeng/dropdown';
import { ButtonModule } from 'primeng/button';
import { TieredMenu } from 'primeng/tieredmenu';
import { AuthClientService } from '../../shared/services/auth-client-service.service';

@Component({
    selector: 'app-topbar',
    standalone: true,
    imports: [RouterModule, CommonModule, StyleClassModule, AppConfigurator, DropdownModule, ButtonModule, TieredMenu],
    providers: [MessageService],
    template: ` <div class="layout-topbar">
        <div class="layout-topbar-logo-container">
            <button class="layout-menu-button layout-topbar-action" (click)="layoutService.onMenuToggle()">
                <i class="pi pi-bars"></i>
            </button>
            <a class="layout-topbar-logo" routerLink="/">
            <img class="h-14 w-14 object-cover rounded-full mx-2" src="/assets/images/taskify-logo.png"/>
                <span>Taskify</span>
            </a>
        </div>

        <div class="layout-topbar-actions">
            <div class="layout-config-menu">
                <button type="button" class="layout-topbar-action" (click)="toggleDarkMode()">
                    <i [ngClass]="{ 'pi ': true, 'pi-moon': layoutService.isDarkTheme(), 'pi-sun': !layoutService.isDarkTheme() }"></i>
                </button>
                <div class="relative">
                    <button
                        class="layout-topbar-action layout-topbar-action-highlight"
                        pStyleClass="@next"
                        enterFromClass="hidden"
                        enterActiveClass="animate-scalein"
                        leaveToClass="hidden"
                        leaveActiveClass="animate-fadeout"
                        [hideOnOutsideClick]="true"
                    >
                        <i class="pi pi-palette"></i>
                    </button>
                    <app-configurator />
                </div>
            </div>

            <button class="layout-topbar-menu-button layout-topbar-action" pStyleClass="@next" enterFromClass="hidden" enterActiveClass="animate-scalein" leaveToClass="hidden" leaveActiveClass="animate-fadeout" [hideOnOutsideClick]="true">
                <i class="pi pi-ellipsis-v"></i>
            </button>

           <div class="layout-topbar-menu hidden lg:block">
                <div class="layout-topbar-menu-content">
                    <button type="button" pButton (click)="menu.toggle($event)">
                        <span class="mr-2"> {{ authClientService.getUserFullName() }}</span>
                        <i class="pi pi-chevron-down"></i>
                    </button>
                    <p-tieredMenu #menu [model]="profileMenu" [popup]="true"></p-tieredMenu>
                </div>
            </div>
        </div>
    </div>`
})
export class AppTopbar {
    
    constructor(
        public layoutService: LayoutService, 
        public toastService: ToastService, 
        public authClientService: AuthClientService) {}

    items!: MenuItem[];
    profileMenu: MenuItem[] = [
        {
            label: 'Profile',
            icon: 'pi pi-user',
            command: () => {
            // navigate to profile page or perform some action
            }
        },
        {
            label: 'Logout',
            icon: 'pi pi-sign-out',
            command: () => {
                this.authClientService.logout();
            }
        }
    ];

    toggleSuccess(){
        this.toastService.showSuccess('Success Message');
    }

    toggleDarkMode() {
        this.layoutService.layoutConfig.update((state) => ({ ...state, darkTheme: !state.darkTheme }));
    }
}
