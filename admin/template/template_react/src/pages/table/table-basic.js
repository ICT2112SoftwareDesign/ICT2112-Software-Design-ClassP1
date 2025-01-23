import React from 'react';
import { Link } from 'react-router-dom';
import { Panel, PanelHeader } from './../../components/panel/panel.jsx';
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
					<li className="breadcrumb-item"><Link to="/table/basic">Home</Link></li>
					<li className="breadcrumb-item"><Link to="/table/basic">Tables</Link></li>
					<li className="breadcrumb-item active">Basic Tables</li>
				</ol>
				<h1 className="page-header">Basic Tables <small>header small text goes here...</small></h1>
				<div className="row">
					<div className="col-lg-6">
						<Panel>
							<PanelHeader>Default Table</PanelHeader>
							<div className="table-responsive">
								<table className="table m-b-0">
									<thead>
										<tr>
											<th>#</th>
											<th>Username</th>
											<th>Email Address</th>
										</tr>
									</thead>
									<tbody>
										<tr>
											<td>1</td>
											<td>Nicky Almera</td>
											<td>nicky@hotmail.com</td>
										</tr>
										<tr>
											<td>2</td>
											<td>Edmund Wong</td>
											<td>edmund@yahoo.com</td>
										</tr>
										<tr>
											<td>3</td>
											<td>Harvinder Singh</td>
											<td>harvinder@gmail.com</td>
										</tr>
									</tbody>
								</table>
							</div>
							<CodeMirror options={this.codeMirrorOptions} value={
'<table className="table">\n'+
'	...\n'+
'</table>'} />
						</Panel>
						
						<Panel>
							<PanelHeader>Hover Table</PanelHeader>
							<div className="table-responsive">
								<table className="table table-hover m-b-0 text-inverse">
									<thead>
										<tr>
											<th>#</th>
											<th>Username</th>
											<th>Email Address</th>
										</tr>
									</thead>
									<tbody>
										<tr>
											<td>1</td>
											<td>Nicky Almera</td>
											<td>nicky@hotmail.com</td>
										</tr>
										<tr>
											<td>2</td>
											<td>Edmund Wong</td>
											<td>edmund@yahoo.com</td>
										</tr>
										<tr>
											<td>3</td>
											<td>Harvinder Singh</td>
											<td>harvinder@gmail.com</td>
										</tr>
									</tbody>
								</table>
							</div>
							<CodeMirror options={this.codeMirrorOptions} value={
'<table className="table table-hover">\n'+
'	...\n'+
'</table>'} />
						</Panel>
						
						<Panel>
							<PanelHeader>Condensed Table</PanelHeader>
							<div className="table-responsive">
								<table className="table table-condensed m-b-0 text-inverse">
									<thead>
										<tr>
											<th>#</th>
											<th>Username</th>
											<th>Email Address</th>
										</tr>
									</thead>
									<tbody>
										<tr>
											<td>1</td>
											<td>Nicky Almera</td>
											<td>nicky@hotmail.com</td>
										</tr>
										<tr>
											<td>2</td>
											<td>Edmund Wong</td>
											<td>edmund@yahoo.com</td>
										</tr>
										<tr>
											<td>3</td>
											<td>Harvinder Singh</td>
											<td>harvinder@gmail.com</td>
										</tr>
									</tbody>
								</table>
							</div>
							<CodeMirror options={this.codeMirrorOptions} value={
'<table className="table table-condensed">\n'+
'	...\n'+
'</table>'} />
						</Panel>
						
						<Panel>
							<PanelHeader>Responsive Table</PanelHeader>
							<div className="table-responsive">
								<table className="table">
									<thead>
										<tr>
											<th>#</th>
											<th className="text-nowrap">Table heading</th>
											<th className="text-nowrap">Table heading</th>
											<th className="text-nowrap">Table heading</th>
											<th className="text-nowrap">Table heading</th>
											<th className="text-nowrap">Table heading</th>
											<th className="text-nowrap">Table heading</th>
										</tr>
									</thead>
									<tbody>
										<tr>
											<td>1</td>
											<td>Table cell</td>
											<td>Table cell</td>
											<td>Table cell</td>
											<td>Table cell</td>
											<td>Table cell</td>
											<td>Table cell</td>
										</tr>
										<tr>
											<td>2</td>
											<td>Table cell</td>
											<td>Table cell</td>
											<td>Table cell</td>
											<td>Table cell</td>
											<td>Table cell</td>
											<td>Table cell</td>
										</tr>
										<tr>
											<td>3</td>
											<td>Table cell</td>
											<td>Table cell</td>
											<td>Table cell</td>
											<td>Table cell</td>
											<td>Table cell</td>
											<td>Table cell</td>
										</tr>
									</tbody>
								</table>
							</div>
							<CodeMirror options={this.codeMirrorOptions} value={
'<div className="table-responsive">\n'+
'	<table className="table">\n'+
'		...\n'+
'	</table>\n'+
'</div>'} />
						</Panel>
						
						<Panel>
							<PanelHeader>Table Striped</PanelHeader>
							<div className="table-responsive">
								<table className="table table-striped m-b-0">
									<thead>
										<tr>
											<th>#</th>
											<th>Username</th>
											<th>Email Address</th>
										</tr>
									</thead>
									<tbody>
										<tr>
											<td>1</td>
											<td>Nicky Almera</td>
											<td>nicky@hotmail.com</td>
										</tr>
										<tr>
											<td>2</td>
											<td>Edmund Wong</td>
											<td>edmund@yahoo.com</td>
										</tr>
										<tr>
											<td>3</td>
											<td>Harvinder Singh</td>
											<td>harvinder@gmail.com</td>
										</tr>
									</tbody>
								</table>
							</div>
							<CodeMirror options={this.codeMirrorOptions} value={
'<table className="table table-striped">\n'+
'	...\n'+
'</table>'} />
						</Panel>
						
						<Panel>
							<PanelHeader>Bordered Table</PanelHeader>
							<div className="table-responsive">
								<table className="table table-bordered m-b-0">
									<thead>
										<tr>
											<th>#</th>
											<th>Username</th>
											<th>Email Address</th>
										</tr>
									</thead>
									<tbody>
										<tr>
											<td>1</td>
											<td>Nicky Almera</td>
											<td>nicky@hotmail.com</td>
										</tr>
										<tr>
											<td>2</td>
											<td>Edmund Wong</td>
											<td>edmund@yahoo.com</td>
										</tr>
										<tr>
											<td>3</td>
											<td>Harvinder Singh</td>
											<td>harvinder@gmail.com</td>
										</tr>
									</tbody>
								</table>
							</div>
							<CodeMirror options={this.codeMirrorOptions} value={
'<table className="table table-bordered">\n'+
'	...\n'+
'</table>'} />
						</Panel>
					</div>
					
					<div className="col-lg-6">
						<Panel>
							<PanelHeader>UI Elements in Table <span className="label label-success m-l-5 t-minus-1">NEW</span></PanelHeader>
							<div className="table-responsive">
								<table className="table table-striped m-b-0">
									<thead>
										<tr>
											<th>#</th>
											<th>Username</th>
											<th>Email Address</th>
											<th width="1%"></th>
										</tr>
									</thead>
									<tbody>
										<tr>
											<td className="with-img">
												<img src="/assets/img/user/user-1.jpg" className="img-rounded height-30" alt="" />
											</td>
											<td>Nicky Almera</td>
											<td>nicky@hotmail.com</td>
											<td className="with-btn text-nowrap">
												<button className="btn btn-sm btn-primary width-60 m-r-2">Edit</button>
												<button className="btn btn-sm btn-white width-60">Delete</button>
											</td>
										</tr>
										<tr>
											<td className="with-img">
												<img src="/assets/img/user/user-2.jpg" className="img-rounded height-30" alt="" />
											</td>
											<td>Edmund Wong</td>
											<td>edmund@yahoo.com</td>
											<td className="with-btn-group text-nowrap">
												<div className="btn-group">
													<button className="btn btn-white btn-sm width-90">Settings</button>
													<button className="btn btn-white btn-sm dropdown-toggle width-30 no-caret" data-toggle="dropdown">
														<span className="caret"></span>
													</button>
													<ul className="dropdown-menu pull-right">
														<li><Link to="/table/basic">Action 1</Link></li>
														<li><Link to="/table/basic">Action 2</Link></li>
														<li><Link to="/table/basic">Action 3</Link></li>
														<li className="divider"></li>
														<li><Link to="/table/basic">Action 4</Link></li>
													</ul>
												</div>
											</td>
										</tr>
										<tr>
											<td className="with-img">
												<img src="/assets/img/user/user-3.jpg" className="img-rounded height-30" alt="" />
											</td>
											<td>Harvinder Singh</td>
											<td>harvinder@gmail.com</td>
											<td className="with-btn text-nowrap">
												<button className="btn btn-sm btn-primary width-60 m-r-2">Edit</button>
												<button className="btn btn-sm btn-white width-60">Delete</button>
											</td>
										</tr>
									</tbody>
								</table>
							</div>
							<CodeMirror options={this.codeMirrorOptions} value={
'<table className="table">\n'+
'	<tbody>\n'+
'		<tr>\n'+
'			<!-- with image -->\n'+
'			<td className="with-img">...</td>\n'+
'\n'+
'			<!-- with button -->\n'+
'			<td className="with-btn">...</td>\n'+
'\n'+
'			<!-- with button group -->\n'+
'			<td className="with-btn-group">...</td>\n'+
'		 </tr>\n'+
'	</tbody>\n'+
'</table>'} />
						</Panel>
						
						<Panel>
							<PanelHeader>Form Elements in Table <span className="label label-success m-l-5 t-minus-1">NEW</span></PanelHeader>
							<div className="table-responsive">
								<table className="table table-striped m-b-0">
									<thead>
										<tr>
											<th className="with-checkbox">
												<div className="checkbox checkbox-css">
													<input type="checkbox" value="" id="table_checkbox_1" />
													<label htmlFor="table_checkbox_1">&nbsp;</label>
												</div>
											</th>
											<th>Username</th>
											<th>Email Address</th>
										</tr>
									</thead>
									<tbody>
										<tr>
											<td className="with-checkbox">
												<div className="checkbox checkbox-css">
													<input type="checkbox" value="" id="table_checkbox_2" defaultChecked />
													<label htmlFor="table_checkbox_2">&nbsp;</label>
												</div>
											</td>
											<td>Nicky Almera</td>
											<td>nicky@hotmail.com</td>
										</tr>
										<tr>
											<td className="with-radio">
												<div className="radio radio-css">
													<input type="radio" id="table_radio_1" name="table_radio" defaultChecked />
													<label htmlFor="table_radio_1">&nbsp;</label>
												</div>
											</td>
											<td>Edmund Wong</td>
											<td>edmund@yahoo.com</td>
										</tr>
										<tr>
											<td className="with-radio">
												<div className="radio radio-css">
													<input type="radio" id="table_radio_2" name="table_radio" />
													<label htmlFor="table_radio_2">&nbsp;</label>
												</div>
											</td>
											<td className="with-form-control"><input type="text" className="form-control" defaultValue="Harvinder Singh" /></td>
											<td>harvinder@gmail.com</td>
										</tr>
										<tr>
											<td className="with-radio">
												<div className="radio radio-css">
													<input type="radio" id="table_radio_3" name="table_radio" />
													<label htmlFor="table_radio_3">&nbsp;</label>
												</div>
											</td>
											<td className="with-input-group">
												<div className="input-group m-b-10">
													<span className="input-group-addon">@</span>
													<input type="text" className="form-control" placeholder="Terry" />
												</div>
											</td>
											<td>terry@gmail.com</td>
										</tr>
									</tbody>
								</table>
							</div>
							<CodeMirror options={this.codeMirrorOptions} value={
'<table className="table">\n'+
'	<tbody>\n'+
'		<tr>\n'+
'			<!-- with checkbox -->\n'+
'			<td className="with-checkbox">...</td>\n'+
'\n'+
'			<!-- with radio -->\n'+
'			<td className="with-radio">...</td>\n'+
'\n'+
'			<!-- with form input -->\n'+
'			<td className="with-form-control">...</td>\n'+
'\n'+
'			<!-- with input group -->\n'+
'			<td className="with-input-control">...</td>\n'+
'		</tr>\n'+
'	</tbody>\n'+
'</table>'} />
						</Panel>
						
						<Panel>
							<PanelHeader>Table Row Classes</PanelHeader>
							<div className="table-responsive">
								<table className="table m-b-0">
									<thead>
										<tr>
											<th>#</th>
											<th>Username</th>
											<th>Email Address</th>
										</tr>
									</thead>
									<tbody>
										<tr className="active">
											<td>1</td>
											<td>Nicky Almera</td>
											<td>nicky@hotmail.com</td>
										</tr>
										<tr className="info">
											<td>2</td>
											<td>Terry Khoo</td>
											<td>terry@gmail.com</td>
										</tr>
										<tr className="success">
											<td>3</td>
											<td>Edmund Wong</td>
											<td>edmund@yahoo.com</td>
										</tr>
										<tr className="warning">
											<td>4</td>
											<td>Harvinder Singh</td>
											<td>harvinder@gmail.com</td>
										</tr>
										<tr className="danger">
											<td>5</td>
											<td>Terry Khoo</td>
											<td>terry@gmail.com</td>
										</tr>
									</tbody>
								</table>
							</div>
							<CodeMirror options={this.codeMirrorOptions} value={
'<table className="table">\n'+
'	<tbody>\n'+
'		<tr className="active">...</tr>\n'+
'		<tr className="info">...</tr>\n'+
'		<tr className="success">...</tr>\n'+
'		<tr className="warning">...</tr>\n'+
'		<tr className="danger">...</tr>\n'+
'	</tbody>\n'+
'</table>'} />
						</Panel>
					</div>
				</div>
			</div>
		)
	}
}

export default UIGeneral;