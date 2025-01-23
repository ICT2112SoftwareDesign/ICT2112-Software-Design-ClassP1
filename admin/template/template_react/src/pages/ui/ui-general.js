import React from 'react';
import { Link } from 'react-router-dom';
import { Panel, PanelHeader, PanelBody } from './../../components/panel/panel.jsx';
import { UnControlled as CodeMirror } from 'react-codemirror2'
require('codemirror/mode/xml/xml.js');
require('codemirror/mode/javascript/javascript.js');

class UIGeneral extends React.Component {
	constructor(props) {
		super(props);
		
		this.codeMirrorOptions = {
			mode: 'application/xml',
			theme: 'material',
			lineNumbers: true,
			indentWithTabs: true,
			tabSize: 2,
			autoScroll: false
		}
	}
	
	render() {
		return (
			<div>
				<ol className="breadcrumb pull-right">
					<li className="breadcrumb-item"><Link to="/">Home</Link></li>
					<li className="breadcrumb-item"><Link to="/ui">UI Elements</Link></li>
					<li className="breadcrumb-item active">General</li>
				</ol>
				<h1 className="page-header">General UI Elements <small>header small text goes here...</small></h1>
		
				<div className="row">
					<div className="col-lg-6">
						<Panel>
							<PanelHeader>Alerts</PanelHeader>
							<PanelBody>
								<div className="alert alert-success fade show m-b-0">
									<span className="close" data-dismiss="alert">&times;</span>
									<strong>Success!</strong>
									This is a success alert with <Link to="/ui/general" className="alert-link">an example link</Link>.
								</div>
							</PanelBody>
							<CodeMirror options={this.codeMirrorOptions} value={
'<div className="alert alert-success fade show">\n'+
'	<span className="close" data-dismiss="alert">&times;</span>\n'+
'	<strong>Success!</strong>\n'+
'	This is a success alert with \n'+
'	<Link to="/ui/general" className="alert-link">an example link</Link>. \n'+
'</div>'}
							/>
						</Panel>
						<Panel>
							<PanelHeader>Alerts Color <span className="label label-success">NEW</span></PanelHeader>
							<PanelBody>
								<div className="row row-space-10">
									<div className="col-md-4">
										<div className="alert alert-primary fade show m-b-10">
											<span className="close" data-dismiss="alert">&times;</span>
											Primary alert with <Link to="/ui/general" className="alert-link">an example link</Link>.
										</div>
									</div>
									<div className="col-md-4">
										<div className="alert alert-info fade show m-b-10">
											<span className="close" data-dismiss="alert">&times;</span>
											Info alert with <Link to="/ui/general" className="alert-link">an example link</Link>.
										</div>
									</div>
									<div className="col-md-4">
										<div className="alert alert-purple fade show m-b-10">
											<span className="close" data-dismiss="alert">&times;</span>
											Purple alert with <Link to="/ui/general" className="alert-link">an example link</Link>.
										</div>
									</div>
									<div className="col-md-4">
										<div className="alert alert-indigo fade show m-b-10">
											<span className="close" data-dismiss="alert">&times;</span>
											Indigo alert with <Link to="/ui/general" className="alert-link">an example link</Link>.
										</div>
									</div>
									<div className="col-md-4">
										<div className="alert alert-success fade show m-b-10">
											<span className="close" data-dismiss="alert">&times;</span>
											Success alert with <Link to="/ui/general" className="alert-link">an example link</Link>.
										</div>
									</div>
									<div className="col-md-4">
										<div className="alert alert-green fade show m-b-10">
											<span className="close" data-dismiss="alert">&times;</span>
											Green alert with <Link to="/ui/general" className="alert-link">an example link</Link>.
										</div>
									</div>
									<div className="col-md-4">
										<div className="alert alert-lime fade show m-b-10">
											<span className="close" data-dismiss="alert">&times;</span>
											Lime alert with <Link to="/ui/general" className="alert-link">an example link</Link>.
										</div>
									</div>
									<div className="col-md-4">
										<div className="alert alert-warning fade show m-b-10">
											<span className="close" data-dismiss="alert">&times;</span>
											Warning alert with <Link to="/ui/general" className="alert-link">an example link</Link>.
										</div>
									</div>
									<div className="col-md-4">
										<div className="alert alert-yellow fade show m-b-10">
											<span className="close" data-dismiss="alert">&times;</span>
											Yellow alert with <Link to="/ui/general" className="alert-link">an example link</Link>.
										</div>
									</div>
									<div className="col-md-4">
										<div className="alert alert-danger fade show m-b-10">
											<span className="close" data-dismiss="alert">&times;</span>
											Danger alert with <Link to="/ui/general" className="alert-link">an example link</Link>.
										</div>
									</div>
									<div className="col-md-4">
										<div className="alert alert-pink fade show m-b-10">
											<span className="close" data-dismiss="alert">&times;</span>
											Pink alert with <Link to="/ui/general" className="alert-link">an example link</Link>.
										</div>
									</div>
									<div className="col-md-4">
										<div className="alert alert-dark fade show m-b-10">
											<span className="close" data-dismiss="alert">&times;</span>
											Dark alert with <Link to="/ui/general" className="alert-link">an example link</Link>.
										</div>
									</div>
									<div className="col-md-4">
										<div className="alert alert-secondary fade show m-b-10">
											<span className="close" data-dismiss="alert">&times;</span>
											Secondary alert with <Link to="/ui/general" className="alert-link">an example link</Link>.
										</div>
									</div>
									<div className="col-md-4">
										<div className="alert alert-light fade show m-b-10">
											<span className="close" data-dismiss="alert">&times;</span>
											Light alert with <Link to="/ui/general" className="alert-link">an example link</Link>.
										</div>
									</div>
								</div>
							</PanelBody>
							<CodeMirror options={this.codeMirrorOptions} value={
'<div className="alert alert-primary fade show">...</div>\n'+
'<div className="alert alert-secondary fade show">...</div>\n'+
'<div className="alert alert-success fade show">...</div>\n'+
'<div className="alert alert-danger fade show">...</div>\n'+
'<div className="alert alert-warning fade show">...</div>\n'+
'<div className="alert alert-yellow fade show">...</div>\n'+
'<div className="alert alert-info fade show">...</div>\n'+
'<div className="alert alert-lime fade show">...</div>\n'+
'<div className="alert alert-purple fade show">...</div>\n'+
'<div className="alert alert-light fade show">...</div>\n'+
'<div className="alert alert-dark fade show">...</div>\n'+
'<div className="alert alert-indigo fade show">...</div>\n'+
'<div className="alert alert-pink fade show">...</div>\n'+
'<div className="alert alert-green fade show">...</div>'}
							/>
						</Panel>
						<Panel>
							<PanelHeader>Notes</PanelHeader>
							<PanelBody>
								<div className="note note-primary m-b-15">
									<div className="note-icon"><i className="fab fa-facebook-f"></i></div>
									<div className="note-content">
										<h4><b>Note with icon!</b></h4>
										<p>
											Lorem ipsum dolor sit amet, consectetur adipiscing elit. 
											Maecenas id gravida libero. Etiam semper id sem a ultricies.
										</p>
									</div>
								</div>
								<div className="note note-warning note-with-right-icon m-b-15">
									<div className="note-icon"><i className="fa fa-lightbulb"></i></div>
									<div className="note-content text-right">
										<h4><b>Note with right icon!</b></h4>
										<p>
											Lorem ipsum dolor sit amet, consectetur adipiscing elit. 
											Maecenas id gravida libero. Etiam semper id sem a ultricies.
										</p>
									</div>
								</div>
								<div className="note note-secondary m-b-15">
									<h4><b>Note without icon!</b></h4>
									<p>
										Lorem ipsum dolor sit amet, consectetur adipiscing elit. 
										Maecenas id gravida libero. Etiam semper id sem a ultricies.
									</p>
								</div>
							</PanelBody>
							<CodeMirror options={this.codeMirrorOptions} value={
'<!-- default -->\n'+
'<div className="note note-primary">\n'+
'	<div className="note-icon"><i className="fab fa-facebook-f"></i></div>\n'+
'	<div className="note-content">\n'+
'		<h4><b>Note with icon!</b></h4>\n'+
'		<p> ... </p>\n'+
'	</div>\n'+
'</div>\n'+
'\n'+
'<!-- with right icon -->\n'+
'<div className="note note-warning note-with-right-icon">\n'+
'	<div className="note-icon"><i className="fa fa-lightbulb"></i></div>\n'+
'	<div className="note-content text-right">\n'+
'		<h4><b>Note with right icon!</b></h4>\n'+
'		<p> ... </p>\n'+
'	</div>\n'+
'</div>'}
							/>
						</Panel>
					</div>
					<div className="col-lg-6">
						<Panel>
							<PanelHeader>
								Labels & Badges <span className="label label-success">NEW</span>
							</PanelHeader>
							<PanelBody>
								<div className="row f-s-15">
									<div className="col-md-8">
										<div className="m-b-3">
											<span className="label label-danger m-r-3">Danger</span>
											<span className="label label-warning m-r-3">Warning</span>
											<span className="label label-yellow m-r-3">Yellow</span>
											<span className="label label-lime m-r-3">Lime</span>
											<span className="label label-green m-r-3">Green</span>
											<span className="label label-success">Success</span>
										</div>
										<div className="m-b-3">
											<span className="label label-primary m-r-3">Primary</span>
											<span className="label label-info m-r-3">Info</span>
											<span className="label label-purple m-r-3">Purple</span>
											<span className="label label-indigo m-r-3">Indigo</span>
											<span className="label label-dark">Dark</span>
										</div>
										<div className="m-b-3">
											<span className="label label-pink m-r-3">Pink</span>
											<span className="label label-secondary m-r-3">Secondary</span>
											<span className="label label-default m-r-3">Default</span>
											<span className="label label-light">Light</span>
										</div>
									</div>
									<div className="col-md-4">
										<div className="m-b-3">
											<span className="label label-inverse">label</span>
										</div>
										<div className="m-b-3">
											<span className="badge badge-secondary">badge</span>
										</div>
										<div className="m-b-3">
											<span className="badge badge-default badge-square">badge-square</span>
										</div>
									</div>
								</div>
							</PanelBody>
							<CodeMirror options={this.codeMirrorOptions} value={
'<!-- label -->\n'+
'<span className="label label-inverse">label</span>\n'+
'\n'+
'<!-- badge -->\n'+
'<span className="badge badge-secondary">badge</span>\n'+
'\n'+
'<!-- badge-square -->\n'+
'<span className="badge badge-default badge-square">badge-square</span>'}
						/>
						</Panel>
						
						<Panel>
							<PanelHeader>Pagination & Pager</PanelHeader>
							<PanelBody>
								<div>
									<ul className="pagination pagination-lg m-t-0 m-b-5">
										<li className="page-item disabled"><Link to="/ui/general" className="page-link">«</Link></li>
										<li className="page-item active"><Link to="/ui/general" className="page-link">1</Link></li>
										<li className="page-item"><Link to="/ui/general" className="page-link">2</Link></li>
										<li className="page-item"><Link to="/ui/general" className="page-link">3</Link></li>
										<li className="page-item"><Link to="/ui/general" className="page-link">4</Link></li>
										<li className="page-item"><Link to="/ui/general" className="page-link">5</Link></li>
										<li className="page-item"><Link to="/ui/general" className="page-link">»</Link></li>
									</ul>
								</div>
								<div>
									<ul className="pagination m-t-0 m-b-5">
										<li className="page-item disabled"><Link to="/ui/general" className="page-link">«</Link></li>
										<li className="page-item active"><Link to="/ui/general" className="page-link">1</Link></li>
										<li className="page-item"><Link to="/ui/general" className="page-link">2</Link></li>
										<li className="page-item"><Link to="/ui/general" className="page-link">3</Link></li>
										<li className="page-item"><Link to="/ui/general" className="page-link">4</Link></li>
										<li className="page-item"><Link to="/ui/general" className="page-link">5</Link></li>
										<li className="page-item"><Link to="/ui/general" className="page-link">»</Link></li>
									</ul>
								</div>
								<div>
									<ul className="pagination pagination-sm m-t-0 m-b-5">
										<li className="page-item disabled"><Link to="/ui/general" className="page-link">«</Link></li>
										<li className="page-item active"><Link to="/ui/general" className="page-link">1</Link></li>
										<li className="page-item"><Link to="/ui/general" className="page-link">2</Link></li>
										<li className="page-item"><Link to="/ui/general" className="page-link">3</Link></li>
										<li className="page-item"><Link to="/ui/general" className="page-link">4</Link></li>
										<li className="page-item"><Link to="/ui/general" className="page-link">5</Link></li>
										<li className="page-item"><Link to="/ui/general" className="page-link">»</Link></li>
									</ul>
								</div>
								<ul className="pager m-b-10">
									<li><Link to="/ui/general">Previous</Link></li>
									<li><Link to="/ui/general">Next</Link></li>
								</ul>
								<ul className="pager m-t-0 m-b-0">
									<li className="previous disabled"><Link to="/ui/general">&larr; Older</Link></li>
									<li className="next"><Link to="/ui/general">Newer &rarr;</Link></li>
								</ul>
							</PanelBody>
							<CodeMirror options={this.codeMirrorOptions} value={
'<!-- pagination -->\n'+
'<ul className="pagination">\n'+
'	<li className="page-item disabled"><a href="#" className="page-link">«</a></li>\n'+
'	<li className="page-item active"><a href="#" className="page-link">1</a></li>\n'+
'	<li className="page-item"><a href="#" className="page-link">2</a></li>\n'+
'	<li className="page-item"><a href="#" className="page-link">3</a></li>\n'+
'	<li className="page-item"><a href="#" className="page-link">4</a></li>\n'+
'	<li className="page-item"><a href="#" className="page-link">5</a></li>\n'+
'	<li className="page-item"><a href="#" className="page-link">»</a></li>\n'+
'</ul>\n'+
'\n'+
'<!-- pager -->\n'+
'<ul className="pager">\n'+
'	<li><a href="#">Previous</a></li>\n'+
'	<li><a href="#">Next</a></li>\n'+
'</ul>'}
						/>
						</Panel>
						
						<Panel>
							<PanelHeader>Progress bar</PanelHeader>
							<PanelBody>
								<div className="row">
									<div className="col-md-6">
										<div className="progress rounded-corner m-b-15">
											<div className="progress-bar" style={{width: '80%'}}>Basic</div>
										</div>
										<div className="progress rounded-corner m-b-15">
											<div className="progress-bar bg-warning progress-bar-striped" style={{width: '80%'}}>Striped</div>
										</div>
									</div>
									<div className="col-md-6">
										<div className="progress rounded-corner m-b-15">
											<div className="progress-bar bg-purple progress-bar-striped progress-bar-animated" style={{width: '80%'}}>Animated</div>
										</div>
										<div className="progress rounded-corner m-b-15">
											<div className="progress-bar bg-dark">Stacked</div>
											<div className="progress-bar bg-grey">Stacked</div>
											<div className="progress-bar bg-lime">Stacked</div>
										</div>
									</div>
								</div>
							</PanelBody>
							<CodeMirror options={this.codeMirrorOptions} value={
'<!-- default -->\n'+
'<div className="progress rounded-corner">\n'+
'	<div className="progress-bar">Basic</div>\n'+
'</div>\n'+
'\n'+
'<!-- striped -->\n'+
'<div className="progress rounded-corner progress-striped">\n'+
'	<div className="progress-bar bg-warning">\n'+
'		Striped\n'+
'	</div>\n'+
'</div>\n'+
'\n'+
'<!-- animated -->\n'+
'<div className="progress rounded-corner progress-striped active">\n'+
'	<div className="progress-bar bg-purple">\n'+
'		Animated\n'+
'	</div>\n'+
'</div>\n'+
'\n'+
'<!-- stacked -->\n'+
'<div className="progress rounded-corner">\n'+
'	<div className="progress-bar bg-dark">\n'+
'		Stacked\n'+
'	</div>\n'+
'	<div className="progress-bar bg-grey">\n'+
'		Stacked\n'+
'	</div>\n'+
'	<div className="progress-bar bg-lime">\n'+
'		Stacked\n'+
'	</div>\n'+
'</div>'}
							/>
						</Panel>
					</div>
				</div>
			</div>
		)
	}
}

export default UIGeneral;