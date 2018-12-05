import React, { Component } from 'react';
import './Settings.css';
import { getSettings, saveSetting } from '../../api/UserApi';

export default class Settings extends Component {

    constructor(props){
        super(props);
        this.state = {
            searchSettings: {},
            contentSettings: {}
        }
    }

    componentDidMount(){
        getSettings().then((settings) => {
            if(settings.length == 2) {
                for(let i=0; i < 2; i++){
                    if(settings[i].setting === 0){
                        this.setState({searchSettings: settings[i]});
                    } else {
                        this.setState({contentSettings: settings[i]});
                    }
                }
            }
        });
    }

    handleChangeSearch = (e) => {
        let searchSettings = this.state.searchSettings;
        searchSettings.privacySetting = e.target.value;
        this.setState({searchSettings: searchSettings})

    }

    handleChangeContent = (e) => {
        let contentSettings = this.state.contentSettings;
        contentSettings.privacySetting = e.target.value;
        this.setState({contentSettings: contentSettings});
    }

    saveSettings = () => {
        saveSetting(this.state.searchSettings);
        saveSetting(this.state.contentSettings);
    }

    render() {
        return (
            <div className="form-group">
                <div className="settings-item">
                    <label>Search settings:</label>
                    <select className="form-control" value={this.state.searchSettings.privacySetting} onChange={this.handleChangeSearch}>
                      <option value="0">Everyone</option>
                      <option value="1">Friends</option>
                      <option value="2">Connections</option>
                    </select>
                </div>

                <div className="settings-item">
                    <label>Content settings:</label>
                    <select className="form-control" value={this.state.contentSettings.privacySetting} onChange={this.handleChangeContent}>
                      <option value="0">Everyone</option>
                      <option value="1">Friends</option>
                      <option value="2">Connections</option>
                    </select>
                </div>
                <button type="submit" className="btn btn-primary" onClick={this.saveSettings}>Save</button>
            </div>
        );
    }
}
