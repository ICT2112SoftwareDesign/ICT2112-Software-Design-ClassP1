import React from 'react';
import { PageSettings } from './../../config/page-settings.js';

class LoginV1 extends React.Component {
	static contextType = PageSettings;

	componentDidMount() {
		this.context.handleSetPageSidebar(false);
		this.context.handleSetPageHeader(false);
	}

	componentWillUnmount() {
		this.context.handleSetPageSidebar(true);
		this.context.handleSetPageHeader(true);
	}
  
	render() {
		return (
			<div className="login bg-black animated fadeInDown">
				<div className="login-header">
					<div className="brand">
						<span className="logo"></span> <b>Color</b> Admin
						<small>responsive bootstrap 4 admin template</small>
					</div>
					<div className="icon">
						<i className="fa fa-lock"></i>
					</div>
				</div>
				<div className="login-content">
					<form method="POST" className="margin-bottom-0">
						<div className="form-group m-b-20">
							<input type="text" className="form-control form-control-lg inverse-mode" placeholder="Email Address" required />
						</div>
						<div className="form-group m-b-20">
							<input type="password" className="form-control form-control-lg inverse-mode" placeholder="Password" required />
						</div>
						<div className="checkbox checkbox-css m-b-20">
							<input type="checkbox" id="remember_checkbox" /> 
							<label htmlFor="remember_checkbox">
								Remember Me
							</label>
						</div>
						<div className="login-buttons">
							<button type="submit" className="btn btn-success btn-block btn-lg">Sign me in</button>
						</div>
					</form>
				</div>
			</div>
		)
	}
}

export default LoginV1;