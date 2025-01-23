import React from 'react';
import { Link } from 'react-router-dom';
import { UncontrolledDropdown, DropdownToggle, DropdownMenu, DropdownItem, Collapse } from 'reactstrap';

class DropdownMegaMenu extends React.Component {
	render() {
		return (
			<Collapse className="collapse navbar-collapse pull-left" isOpen={this.props.collapse}>
				<ul className="nav navbar-nav">
					<UncontrolledDropdown tag="li" className="dropdown dropdown-lg">
						<DropdownToggle tag="a">
							<i className="fa fa-th-large fa-fw"></i> Mega <b className="caret"></b>
						</DropdownToggle>
						<DropdownMenu className="dropdown-menu-lg">
							<div className="row">
								<div className="col-lg-4 col-md-4">
									<h4 className="dropdown-header">UI Kits</h4>
									<div className="row m-b-20">
										<div className="col-lg-6">
											<ul className="nav d-block">
												<li><Link to="/page-option/with-mega-menu" className="text-ellipsis"><i className="fa fa-angle-right fa-fw fa-lg text-inverse"></i> FontAwesome</Link></li>
												<li><Link to="/page-option/with-mega-menu" className="text-ellipsis"><i className="fa fa-angle-right fa-fw fa-lg text-inverse"></i> Ionicons</Link></li>
												<li><Link to="/page-option/with-mega-menu" className="text-ellipsis"><i className="fa fa-angle-right fa-fw fa-lg text-inverse"></i> Simple Line Icons</Link></li>
												<li><Link to="/page-option/with-mega-menu" className="text-ellipsis"><i className="fa fa-angle-right fa-fw fa-lg text-inverse"></i> Typography</Link></li>
												<li><Link to="/page-option/with-mega-menu" className="text-ellipsis"><i className="fa fa-angle-right fa-fw fa-lg text-inverse"></i> Media Object</Link></li>
												<li><Link to="/page-option/with-mega-menu" className="text-ellipsis"><i className="fa fa-angle-right fa-fw fa-lg text-inverse"></i> Widget Boxes</Link></li>
												<li><Link to="/page-option/with-mega-menu" className="text-ellipsis"><i className="fa fa-angle-right fa-fw fa-lg text-inverse"></i> Tabs & Accordions</Link></li>
											</ul>
										</div>
										<div className="col-lg-6">
											<ul className="nav d-block">
												<li><Link to="/page-option/with-mega-menu" className="text-ellipsis"><i className="fa fa-angle-right fa-fw fa-lg text-inverse"></i> Unlimited Nav Tabs</Link></li>
												<li><Link to="/page-option/with-mega-menu" className="text-ellipsis"><i className="fa fa-angle-right fa-fw fa-lg text-inverse"></i> Modal & Notification</Link></li>
												<li><Link to="/page-option/with-mega-menu" className="text-ellipsis"><i className="fa fa-angle-right fa-fw fa-lg text-inverse"></i> Buttons</Link></li>
											</ul>
										</div>
									</div>
								</div>
								<div className="col-lg-4 col-md-4">
									<h4 className="dropdown-header">Page Options <span className="label label-inverse">11</span></h4>
									<div className="row m-b-20">
										<div className="col-lg-6">
											<ul className="nav d-block">
												<li><Link to="/page-option/with-mega-menu" className="text-ellipsis"><i className="fa fa-angle-right fa-fw fa-lg text-inverse"></i> Blank Page</Link></li>
												<li><Link to="/page-option/with-mega-menu" className="text-ellipsis"><i className="fa fa-angle-right fa-fw fa-lg text-inverse"></i> Page with Footer <span className="badge badge-success">NEW</span></Link></li>
												<li><Link to="/page-option/with-mega-menu" className="text-ellipsis"><i className="fa fa-angle-right fa-fw fa-lg text-inverse"></i> Page without Sidebar</Link></li>
												<li><Link to="/page-option/with-mega-menu" className="text-ellipsis"><i className="fa fa-angle-right fa-fw fa-lg text-inverse"></i> Page with Right Sidebar</Link></li>
												<li><Link to="/page-option/with-mega-menu" className="text-ellipsis"><i className="fa fa-angle-right fa-fw fa-lg text-inverse"></i> Page with Minified Sidebar</Link></li>
												<li><Link to="/page-option/with-mega-menu" className="text-ellipsis"><i className="fa fa-angle-right fa-fw fa-lg text-inverse"></i> Page with Two Sidebar</Link></li>
												<li><Link to="/page-option/with-mega-menu" className="text-ellipsis"><i className="fa fa-angle-right fa-fw fa-lg text-inverse"></i> Page with Line Icons</Link></li>
											</ul>
										</div>
										<div className="col-lg-6">
											<ul className="nav d-block">
												<li><Link to="/page-option/with-mega-menu" className="text-ellipsis"><i className="fa fa-angle-right fa-fw fa-lg text-inverse"></i> Full Height Content</Link></li>
												<li><Link to="/page-option/with-mega-menu" className="text-ellipsis"><i className="fa fa-angle-right fa-fw fa-lg text-inverse"></i> Page with Mega Menu</Link></li>
												<li><Link to="/page-option/with-mega-menu" className="text-ellipsis"><i className="fa fa-angle-right fa-fw fa-lg text-inverse"></i> Page with Light Sidebar</Link></li>
												<li><Link to="/page-option/with-mega-menu" className="text-ellipsis"><i className="fa fa-angle-right fa-fw fa-lg text-inverse"></i> Page with Large Sidebar</Link></li>
											</ul>
										</div>
									</div>
								</div>
								<div className="col-lg-4 col-md-4">
									<h4 className="dropdown-header">Paragraph</h4>
									<p>
										Lorem ipsum dolor sit amet, consectetur adipiscing elit. Duis libero purus, fermentum at libero convallis, auctor dignissim mauris. Nunc laoreet pellentesque turpis sodales ornare. Nunc vestibulum nunc lorem, at sodales velit malesuada congue. Nam est tortor, tincidunt sit amet eros vitae, aliquam finibus mauris.
									</p>
									<p>
										Fusce ac ligula laoreet ante dapibus mattis. Nam auctor vulputate aliquam. Suspendisse efficitur, felis sed elementum eleifend, ipsum tellus sodales nisi, ut condimentum nisi sem in nibh. Phasellus suscipit vulputate purus at venenatis. Quisque luctus tincidunt tempor.
									</p>
								</div>
							</div>
						</DropdownMenu>
					</UncontrolledDropdown>
					<li>
						<Link to="/page-option/with-mega-menu">
							<i className="fa fa-gem fa-fw"></i> Client
						</Link>
					</li>
					<UncontrolledDropdown tag="li" className="dropdown">
						<DropdownToggle tag="a">
							<i className="fa fa-database fa-fw"></i> New <b className="caret"></b>
						</DropdownToggle>
						<DropdownMenu>
							<DropdownItem>Action</DropdownItem>
							<DropdownItem>Another action</DropdownItem>
							<DropdownItem>Something else here...</DropdownItem>
							<DropdownItem divider />
							<DropdownItem>Separated link</DropdownItem>
							<DropdownItem divider />
							<DropdownItem>One more Separated link</DropdownItem>
						</DropdownMenu>
					</UncontrolledDropdown>
				</ul>
			</Collapse>
		)
	}
}

export default DropdownMegaMenu;
