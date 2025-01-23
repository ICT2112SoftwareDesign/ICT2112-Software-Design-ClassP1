import React from 'react';
import { Link } from 'react-router-dom';
import { PageSettings } from './../../config/page-settings.js';

class EmailDetail extends React.Component {
	static contextType = PageSettings;

	constructor(props) {
		super(props);

		this.toggle = this.toggle.bind(this);
		this.state = {
			dropdownOpen: false
		};
	}

	componentDidMount() {
		this.context.handleSetPageContentFullHeight(true);
		this.context.handleSetPageContentFullWidth(true);
	}

	componentWillUnmount() {
		this.context.handleSetPageContentFullHeight(false);
		this.context.handleSetPageContentFullWidth(false);
	}

	toggle() {
		this.setState(prevState => ({
			dropdownOpen: !prevState.dropdownOpen
		}));
	}
  
	render() {
		return (
			<div className="vertical-box with-grid inbox">
				<div className="vertical-box-column width-200 bg-silver hidden-xs">
					<div className="vertical-box">
						<div className="wrapper bg-silver text-center">
							<Link to="/email/compose" className="btn btn-inverse p-l-40 p-r-40 btn-sm">
								Compose
							</Link>
						</div>
						<div className="vertical-box-row">
							<div className="vertical-box-cell">
								<div className="vertical-box-inner-cell">
									<div data-scrollbar="true" data-height="100%">
										<div className="wrapper p-0">
											<div className="nav-title"><b>FOLDERS</b></div>
											<ul className="nav nav-inbox">
												<li><Link to="/email/inbox"><i className="fa fa-inbox fa-fw m-r-5"></i> Inbox <span className="badge pull-right">52</span></Link></li>
												<li><Link to="/email/inbox"><i className="fa fa-flag fa-fw m-r-5"></i> Important</Link></li>
												<li><Link to="/email/inbox"><i className="fa fa-envelope fa-fw m-r-5"></i> Sent</Link></li>
												<li><Link to="/email/inbox"><i className="fa fa-pencil-alt fa-fw m-r-5"></i> Drafts</Link></li>
												<li><Link to="/email/inbox"><i className="fa fa-trash fa-fw m-r-5"></i> Trash</Link></li>
											</ul>
											<div className="nav-title"><b>LABEL</b></div>
											<ul className="nav nav-inbox">
												<li><Link to="/email/inbox"><i className="fa fa-fw f-s-10 m-r-5 fa-circle text-inverse"></i> Admin</Link></li>
												<li><Link to="/email/inbox"><i className="fa fa-fw f-s-10 m-r-5 fa-circle text-primary"></i> Designer & Employer</Link></li>
												<li><Link to="/email/inbox"><i className="fa fa-fw f-s-10 m-r-5 fa-circle text-success"></i> Staff</Link></li>
												<li><Link to="/email/inbox"><i className="fa fa-fw f-s-10 m-r-5 fa-circle text-warning"></i> Sponsorer</Link></li>
												<li><Link to="/email/inbox"><i className="fa fa-fw f-s-10 m-r-5 fa-circle text-danger"></i> Client</Link></li>
											</ul>
										</div>
									</div>
								</div>
							</div>
						</div>
					</div>
				</div>
				<div className="vertical-box-column bg-white">
					<div className="vertical-box">
						<div className="wrapper bg-silver-lighter clearfix">
							<div className="pull-left">
								<div className="btn-group m-r-5">
									<button className="btn btn-white btn-sm"><i className="fa fa-reply f-s-14 m-r-3 m-r-xs-0 t-plus-1"></i> <span className="hidden-xs">Reply</span></button>
								</div>
								<div className="btn-group m-r-5">
									<button className="btn btn-white btn-sm"><i className="fa fa-trash f-s-14 m-r-3 m-r-xs-0 t-plus-1"></i> <span className="hidden-xs">Delete</span></button>
									<button className="btn btn-white btn-sm"><i className="fa fa-archive f-s-14 m-r-3 m-r-xs-0 t-plus-1"></i> <span className="hidden-xs">Archive</span></button>
								</div>
							</div>
							<div className="pull-right">
								<div className="btn-group">
									<router-link to="/email/inbox" className="btn btn-white btn-sm disabled"><i className="fa fa-arrow-up f-s-14 t-plus-1"></i></router-link>
									<router-link to="/email/inbox" className="btn btn-white btn-sm"><i className="fa fa-arrow-down f-s-14 t-plus-1"></i></router-link>
								</div>
								<div className="btn-group m-l-5">
									<router-link to="/email/inbox" className="btn btn-white btn-sm"><i className="fa fa-times f-s-14 t-plus-1"></i></router-link>
								</div>
							</div>
						</div>
						<div className="vertical-box-row">
							<div className="vertical-box-cell">
								<div className="vertical-box-inner-cell">
									<div className="overflow-scroll height-full">
										<div className="wrapper">
											<h3 className="m-t-0 m-b-15 f-w-500">Bootstrap v4.0 is coming soon</h3>
											<ul className="media-list underline m-b-15 p-b-15">
												<li className="media media-sm clearfix">
													<Link to="/email/inbox" className="pull-left">
														<img className="media-object rounded-corner" alt="" src="/assets/img/user/user-12.jpg" />
													</Link>
													<div className="media-body">
														<div className="email-from text-inverse f-s-14 f-w-600 m-b-3">
															from support@wrapbootstrap.com
														</div>
														<div className="m-b-3"><i className="fa fa-clock fa-fw"></i> Today, 8:30 AM</div>
														<div className="email-to">
															To: nguoksiong@live.co.uk
														</div>
													</div>
												</li>
											</ul>
											<ul className="attached-document clearfix">
												<li className="fa-file">
													<div className="document-file">
														<Link to="/email/detail">
															<i className="fa fa-file-pdf"></i>
														</Link>
													</div>
													<div className="document-name"><Link to="/email/detail">flight_ticket.pdf</Link></div>
												</li>
												<li className="fa-camera">
													<div className="document-file">
														<Link to="/email/detail">
															<img src="/assets/img/gallery/gallery-11.jpg" alt="" />
														</Link>
													</div>
													<div className="document-name"><Link to="/email/detail">front_end_mockup.jpg</Link></div>
												</li>
											</ul>

											<p className="f-s-12 text-inverse p-t-10"> 
												Lorem ipsum dolor sit amet, consectetur adipiscing elit. Cras vel auctor nisi, vel auctor orci. <br />
												Aenean in pretium odio, ut lacinia tellus. Nam sed sem ac enim porttitor vestibulum vitae at erat.
											</p>
											<p className="f-s-12 text-inverse">
												Curabitur auctor non orci a molestie. Nunc non justo quis orci viverra pretium id ut est. <br />
												Nullam vitae dolor id enim consequat fermentum. Ut vel nibh tellus. <br />
												Duis finibus ante et augue fringilla, vitae scelerisque tortor pretium. <br />
												Phasellus quis eros erat. Nam sed justo libero.
											</p>
											<p className="f-s-12 text-inverse">
												Class aptent taciti sociosqu ad litora torquent per conubia nostra, per inceptos himenaeos.<br /> 
												Sed tempus dapibus libero ac commodo.
											</p>
											<br />
											<br />
											<p className="f-s-12 text-inverse">
												Best Regards,<br />
												Sean.<br /><br />
												Information Technology Department,<br />
												Senior Front End Designer<br />
											</p>
										</div>
									</div>
								</div>
							</div>
						</div>
						<div className="wrapper bg-silver-lighter text-right clearfix">
							<div className="btn-group">
								<Link to="/email/inbox" className="btn btn-white btn-sm disabled"><i className="fa fa-arrow-up"></i></Link>
								<Link to="/email/inbox" className="btn btn-white btn-sm"><i className="fa fa-arrow-down"></i></Link>
							</div>
							<div className="btn-group m-l-5">
								<Link to="/email/inbox" className="btn btn-white btn-sm"><i className="fa fa-times"></i></Link>
							</div>
						</div>
					</div>
				</div>
			</div>
		)
	}
}

export default EmailDetail;