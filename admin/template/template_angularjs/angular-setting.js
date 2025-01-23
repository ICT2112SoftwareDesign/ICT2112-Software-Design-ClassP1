/*
Template Name: Color Admin - Responsive Admin Dashboard Template build with Twitter Bootstrap 3 & 4
Version: 4.2.0
Author: Sean Ngu
Website: http://www.seantheme.com/color-admin-v4.2/admin/angularjs/
*/

/* Global Setting
------------------------------------------------ */

colorAdminApp.factory('setting', ['$rootScope', function($rootScope) {
	var setting = {
		layout: {
			pageSidebarMinified: false,
			pageFixedFooter: false,
			pageRightSidebar: false,
			pageTwoSidebar: false,
			pageTopMenu: false,
			pageBoxedLayout: false,
			pageWithoutSidebar: false,
			pageContentFullHeight: false,
			pageContentFullWidth: false,
			pageContentInverseMode: false,
			pageSidebarTransparent: false,
			pageWithFooter: false,
			pageLightSidebar: false,
			pageMegaMenu: false,
			pageBgWhite: false,
			pageWithoutHeader: false,
			paceTop: false
		}
	};

	return setting;
}]);