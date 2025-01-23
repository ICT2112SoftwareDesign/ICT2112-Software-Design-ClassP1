import React from 'react';
import { PageSettings } from './config/page-settings.js';

import Header from './components/header/header.jsx';
import Sidebar from './components/sidebar/sidebar.jsx';
import SidebarRight from './components/sidebar-right/sidebar-right.jsx';
import TopMenu from './components/top-menu/top-menu.jsx';
import Content from './components/content/content.jsx';
import Footer from './components/footer/footer.jsx';
import FloatSubMenu from './components/float-sub-menu/float-sub-menu.jsx';


class App extends React.Component {
	constructor(props) {
		super(props);
		
		this.toggleSidebarMinify = (e) => {
			e.preventDefault();
			if (this.state.pageSidebarMinify) {
				this.setState(state => ({
					pageFloatSubMenuActive: false
				}));
			}
			this.setState(state => ({
				pageSidebarMinify: !this.state.pageSidebarMinify
			}));
		}
		this.toggleMobileSidebar = (e) => {
			this.setState(state => ({
				pageSidebarToggled: !this.state.pageSidebarToggled
			}));
		}
		this.handleSetPageSidebar = (value) => {
			this.setState(state => ({
				pageSidebar: value
			}));
		}
		this.handleSetPageSidebarMinified = (value) => {
			this.setState(state => ({
				pageSidebarMinify: value
			}));
		}
		this.handleSetPageSidebarWide = (value) => {
			this.setState(state => ({
				pageSidebarWide: value
			}));
		}
		this.handleSetPageSidebarLight = (value) => {
			this.setState(state => ({
				pageSidebarLight: value
			}));
		}
		this.handleSetPageSidebarTransparent = (value) => {
			this.setState(state => ({
				pageSidebarTransparent: value
			}));
		}
		this.handleSidebarOnMouseOver = (e, menu) => {
			if (this.state.pageSidebarMinify) {
				if (menu.children) {
					clearTimeout(floatSubMenuRemove);
			
					this.setState(state => ({
						pageFloatSubMenu: menu,
						pageFloatSubMenuActive: true
					}));
			
					const windowHeight = window.innerHeight;
					const targetHeight = document.getElementById('float-sub-menu').offsetHeight;
					const targetTop = e.currentTarget.getBoundingClientRect().top;
					const top = ((windowHeight - targetTop) > targetHeight) ? targetTop : 'auto';
					const left = (e.currentTarget.getBoundingClientRect().left + document.getElementById('sidebar').offsetWidth) + 'px';
					const bottom = ((windowHeight - targetTop) > targetHeight) ? 'auto' : '0';
					const arrowTop = ((windowHeight - targetTop) > targetHeight) ? '20px' : 'auto';
					const arrowBottom = ((windowHeight - targetTop) > targetHeight) ? 'auto' : ((windowHeight - targetTop) - 21) + 'px';
					const lineTop = ((windowHeight - targetTop) > targetHeight) ? '20px' : 'auto';
					const lineBottom = ((windowHeight - targetTop) > targetHeight) ? 'auto' : ((windowHeight - targetTop) - 21) + 'px';
			
					this.setState(state => ({
						pageFloatSubMenuTop: top,
						pageFloatSubMenuLeft: left,
						pageFloatSubMenuBottom: bottom,
						pageFloatSubMenuLineTop: lineTop,
						pageFloatSubMenuLineBottom: lineBottom,
						pageFloatSubMenuArrowTop: arrowTop,
						pageFloatSubMenuArrowBottom: arrowBottom
					}));
				} else {
					floatSubMenuRemove = setTimeout(() => {
						this.setState(state => ({
							pageFloatSubMenu: '',
							pageFloatSubMenuActive: false
						}));
					}, floatSubMenuRemoveTime);
				}
			}
		}
		this.handleSidebarOnMouseOut = (e) => {
			if (this.state.pageSidebarMinify) {
				floatSubMenuRemove = setTimeout(() => {
					this.setState(state => ({
						pageFloatSubMenuActive: false
					}));
				}, floatSubMenuRemoveTime);
			}
		}
		
		this.toggleRightSidebar = (e) => {
			e.preventDefault();
			this.setState(state => ({
				pageRightSidebarCollapsed: !this.state.pageRightSidebarCollapsed
			}));
		}
		this.toggleMobileRightSidebar = (e) => {
			e.preventDefault();
			this.setState(state => ({
				pageMobileRightSidebarToggled: !this.state.pageMobileRightSidebarToggled
			}));
		}
		this.handleSetPageRightSidebar = (value) => {
			this.setState(state => ({
				pageRightSidebar: value
			}));
		}
		
		var floatSubMenuRemove;
		var floatSubMenuRemoveTime = 250;
		this.handleFloatSubMenuOnMouseOver = (e) => {
			clearTimeout(floatSubMenuRemove);
		}
		this.handleFloatSubMenuOnMouseOut = (e) => {
			floatSubMenuRemove = setTimeout(() => {
				this.setState(state => ({
					pageFloatSubMenuActive: false
				}));
			}, floatSubMenuRemoveTime);
		}
		
		this.handleSetPageContent = (value) => {
			this.setState(state => ({
				pageContent: value
			}));
		}
		this.handleSetPageContentClass = (value) => {
			this.setState(state => ({
				pageContentClass: value
			}));
		}
		this.handleSetPageContentFullHeight = (value) => {
			this.setState(state => ({
				pageContentFullHeight: value
			}));
		}
		this.handleSetPageContentFullWidth = (value) => {
			this.setState(state => ({
				pageContentFullWidth: value
			}));
		}
		
		this.handleSetPageHeader = (value) => {
			this.setState(state => ({
				pageHeader: value
			}));
		}
		this.handleSetPageHeaderMegaMenu = (value) => {
			this.setState(state => ({
				pageHeaderMegaMenu: value
			}));
		}
		this.handleSetPageHeaderLanguageBar = (value) => {
			this.setState(state => ({
				pageHeaderLanguageBar: value
			}));
		}
		
		this.handleSetPageFooter = (value) => {
			this.setState(state => ({
				pageFooter: value
			}));
		}
		this.handleSetPageTopMenu = (value) => {
			this.setState(state => ({
				pageTopMenu: value
			}));
		}
		this.handleSetPageTwoSidebar = (value) => {
			this.setState(state => ({
				pageTwoSidebar: value
			}));
		}
		this.handleSetPageBoxedLayout = (value) => {
			if (value === true) {
				document.body.classList.add('boxed-layout');
			} else {
				document.body.classList.remove('boxed-layout');
			}
		}
		this.handleSetBodyWhiteBg = (value) => {
			if (value === true) {
				document.body.classList.add('bg-white');
			} else {
				document.body.classList.remove('bg-white');
			}
		}
		
		this.state = {
			pageHeader: true,
			pageheaderMegaMenu: false,
			pageHeaderLanguageBar: false,
			handleSetPageHeader: this.handleSetPageHeader,
			handleSetPageHeaderLanguageBar: this.handleSetPageHeaderLanguageBar,
			handleSetPageHeaderMegaMenu: this.handleSetPageHeaderMegaMenu,
			
			pageSidebar: true,
			pageSidebarWide: false,
			pageSidebarLight: false,
			pageSidebarMinify: false,
			pageSidebarToggled: false,
			pageSidebarTransparent: false,
			handleSetPageSidebar: this.handleSetPageSidebar,
			handleSetPageSidebarWide: this.handleSetPageSidebarWide,
			handleSetPageSidebarLight: this.handleSetPageSidebarLight,
			handleSetPageSidebarMinified: this.handleSetPageSidebarMinified,
			handleSetPageSidebarTransparent: this.handleSetPageSidebarTransparent,
			handleSidebarOnMouseOut: this.handleSidebarOnMouseOut,
			handleSidebarOnMouseOver: this.handleSidebarOnMouseOver,
			toggleSidebarMinify: this.toggleSidebarMinify,
			toggleMobileSidebar: this.toggleMobileSidebar,
			
			pageFloatSubMenuActive: false,
			pageFloatSubMenu: '',
			pageFloatSubMenuTop: 'auto',
			pageFloatSubMenuLeft: 'auto',
			pageFloatSubMenuBottom: 'auto',
			pageFloatSubMenuLineTop: 'auto',
			pageFloatSubMenuLineBottom: 'auto',
			pageFloatSubMenuArrowTop: 'auto',
			pageFloatSubMenuArrowBottom: 'auto',
			handleFloatSubMenuOnMouseOver: this.handleFloatSubMenuOnMouseOver,
			handleFloatSubMenuOnMouseOut: this.handleFloatSubMenuOnMouseOut,
			
			pageContent: true,
			pageContentClass: '',
			pageContentFullHeight: false,
			pageContentFullWidth: false,
			handleSetPageContent: this.handleSetPageContent,
			handleSetPageContentClass: this.handleSetPageContentClass,
			handleSetPageContentFullHeight: this.handleSetPageContentFullHeight,
			handleSetPageContentFullWidth: this.handleSetPageContentFullWidth,
			
			pageFooter: false,
			handleSetPageFooter: this.handleSetPageFooter,
			
			pageTopMenu: false,
			handleSetPageTopMenu: this.handleSetPageTopMenu,
			
			pageTwoSidebar: false,
			handleSetPageTwoSidebar: this.handleSetPageTwoSidebar,
			
			pageRightSidebar: false,
			pageRightSidebarToggled: true,
			pageMobileRightSidebarToggled: false,
			toggleRightSidebar: this.toggleRightSidebar,
			toggleMobileRightSidebar: this.toggleMobileRightSidebar,
			handleSetPageRightSidebar: this.handleSetPageRightSidebar,
			
			handleSetBodyWhiteBg: this.handleSetBodyWhiteBg,
			handleSetPageBoxedLayout: this.handleSetPageBoxedLayout
		};
	}
	
	render() {
		return (
			<PageSettings.Provider value={this.state}>
				<div className={
					'fade page-sidebar-fixed show page-container ' + 
					(this.state.pageHeader ? 'page-header-fixed ' : '') + 
					(this.state.pageSidebar ? '' : 'page-without-sidebar ') + 
					(this.state.pageRightSidebar ? 'page-with-right-sidebar ' : '') +
					(this.state.pageSidebarWide ? 'page-with-wide-sidebar ' : '') +
					(this.state.pageSidebarLight ? 'page-with-light-sidebar ' : '') +
					(this.state.pageSidebarMinify ? 'page-sidebar-minified ' : '') + 
					(this.state.pageSidebarToggled ? 'page-sidebar-toggled ' : '') + 
					(this.state.pageTopMenu ? 'page-with-top-menu ' : '') + 
					(this.state.pageContentFullHeight ? 'page-content-full-height ' : '') + 
					(this.state.pageTwoSidebar ? 'page-with-two-sidebar ' : '') + 
					(this.state.pageRightSidebarCollapsed ? 'page-right-sidebar-collapsed ' : '') + 
					(this.state.pageMobileRightSidebarToggled ? 'page-right-sidebar-toggled ' : '')
				}>
					{this.state.pageHeader && (<Header />)}
					{this.state.pageSidebar && (<Sidebar />)}
					{this.state.pageTwoSidebar && (<SidebarRight />)}
					{this.state.pageTopMenu && (<TopMenu />)}
					{this.state.pageContent && (<Content />)}
					{this.state.pageFooter && (<Footer />)}
					<FloatSubMenu />
				</div>
			</PageSettings.Provider>
		)
	}
}

export default App;