import React, { Component } from 'react';
import './Settings.css';
import { getSettings, saveSettings } from '../../api/UserApi';

export default class Settings extends Component {

    constructor(props){
        super(props);
        this.state = {
            searchSettings: {},
            contentSettings: {},
            isBusy: false,
            isSaved: false
    };
    }

    componentDidMount(){
        getSettings().then((settings) => {
            if(settings.length === 2) {
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
        this.setState({ searchSettings: searchSettings, isSaved: false });
       
    }

    handleChangeContent = (e) => {
        let contentSettings = this.state.contentSettings;
        contentSettings.privacySetting = e.target.value;
        this.setState({contentSettings: contentSettings, isSaved: false});
    }

    saveSettings = () => {
        this.setState({ isBusy: true });
        saveSettings([this.state.searchSettings, this.state.contentSettings])
            .then(() => this.setState({ isBusy: false, isSaved: true }));
    }

    render() {
        return (
            <div className="form-group">
                <div className="settings-item">
                    <label>Kto widzi moj profil:</label>
                    <select className="form-control" value={this.state.searchSettings.privacySetting} onChange={this.handleChangeSearch}>
                      <option value="0">Wszyscy</option>
                      <option value="1">Znajomi</option>
                    </select>
                </div>

                <div className="settings-item">
                    <label>Kto widzi moje posty:</label>
                    <select className="form-control" value={this.state.contentSettings.privacySetting} onChange={this.handleChangeContent}>
                      <option value="0">Wszyscy</option>
                      <option value="1">Znajomi</option>
                    </select>
                </div>
                {!this.state.isBusy && <button type="submit" className="btn btn-primary" disabled={this.state.isSaved} onClick={this.saveSettings}>Zapisz</button>}
                {this.state.isBusy && <div className="cssload-container">
                                          <div className="cssload-whirlpool" />
                                      </div>}
                {this.state.isSaved && <span> Zapisano! </span>}
            </div>
        );
    }
}
