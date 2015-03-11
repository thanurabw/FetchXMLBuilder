LoadPeople = function (apimethod, element, cols) {
    $.ajax({
        url: 'https://api.github.com/repos/' + GH_USER + '/' + GH_REPO + '/' + apimethod,
        success: function (data) {
            var gazers = "";
            if (data) {
                data.sort(sort_by('login', false, function (a) { return a.toUpperCase() }));
                $(data).each(function (index) {
                    gazers += "<a href='" + this.html_url + "' id='" + apimethod + "_" + this.login + "' target='_blank'><img src='" + this.avatar_url + "' height='50' width='50'/></a>";
                    if ((index + 1) % cols == 0) {
                        gazers += "<br/>";
                    }
                    GetUserInfo(this, apimethod);
                });
            }
            if (gazers) {
                gazers = //"--starred by--<br/>" +
                    gazers + "<br/><br/>";
            }
            $("#" + element).html(gazers);
        }
    });
};

GetUserInfo = function (user, target) {
    $.ajax({
        url: 'https://api.github.com/users/' + user.login,
        success: function (data) {
            var info = "";
            if (data) {
                if (data.name) {
                    info = data.name + " (" + user.login + ")";
                } else {
                    info = user.login;
                }
                if (data.company) {
                    info += "\n" + data.company;
                }
                if (data.location) {
                    info += "\n" + data.location;
                }
                if (data.hireable) {
                    info += "\n\nFOR HIRE!";
                }
            }
            var element = "#" + target + "_" + user.login;
            $(element).attr('title', info);
        }
    });
};

UpdateDownloads = function (version, published, currentcount, releaselink) {
    $("#" + version).text("Loading...");
    $("#" + published).text("Loading...");
    $("#" + currentcount).text("Loading...");
    $.ajax({
        url: 'https://api.github.com/repos/' + GH_USER + '/' + GH_REPO + '/releases/latest',
        success: function (data) {
            if (data && data.assets && data.assets.length > 0) {
                var count = data.assets[0].download_count;
                var tag = data.tag_name;
                if (tag == "1.2015.1.10") {
                    // Add codeplex count
                    count += 339;
                    // Del test count
                    count -= 12;
                }
                var date = new Date(data.published_at);
                $("#" + version).text(data.tag_name);
                $("#" + published).text(date.toFormattedString('yyyy-mm-dd'));
                $("#" + currentcount).text(count);
                $("#latest-download span").text("Download FXB " + tag);
                $("#latest-download").attr('href', data.assets[0].browser_download_url);
                $("#" + releaselink).attr('href', data.html_url);
                //var notes = data.body;
                //var converter = new Showdown.converter();
                //var htmlnotes = converter.makeHtml(notes);
                //$("#latest-notes").text(htmlnotes);
            } else {
                $("#" + version).text("");
                $("#" + published).text("");
                $("#" + currentcount).text("");
                $("#latest-download span").text("No download available");
                $("#latest-download").attr('href', "#");
            }
        },
        error: function (xhr, options, error) {
            $("#" + published).text("");
            $("#" + currentcount).text("");
            if (xhr && xhr.status && xhr.status == 403) {
                $("#" + version).text("");
                //if (xhr.responseText) {
                //    var response = JSON.parse(xhr.responseText);
                //    if (response.message) {
                //        $("#latest-version").text(response.message);
                //    }
                //}
                $("#latest-download span").text("You really want FXB, right!?");
                $("#latest-download").attr('href', 'https://github.com/' + GH_USER + '/' + GH_REPO + '/releases');
            }
            else {
                $("#" + version).text(error);
                $("#latest-download span").text("");
                $("#latest-download").attr('href', "#");
            }
        }
    });
};

GetLatestDownloadLink = function () {
    var url = "";
    $.ajax({
        async: false,
        url: 'https://api.github.com/repos/' + GH_USER + '/' + GH_REPO + '/releases/latest',
        success: function (data) {
            if (data && data.assets && data.assets.length > 0) {
                url = data.assets[0].browser_download_url;
            }
        },
        error: function (xhr, options, error) {
            return "/";
        }
    });

    return url;
};

UpdateTotalDownloads = function (totalcount) {
    $("#" + totalcount).text("");
    $.ajax({
        url: 'https://api.github.com/repos/' + GH_USER + '/' + GH_REPO + '/releases',
        success: function (data) {
            if (data && data.length > 0) {
                var count = 0;
                $(data).each(function (index) {
                    if (this.assets.length > 0) {
                        count += this.assets[0].download_count;
                    }
                });
                // Add codeplex count
                count += 858;
                $("#" + totalcount).text(" (" + count + ")");
            }
        }
    });
};

UpdateReleaseNotes = function (releasenotes, callback) {
    $("#" + releasenotes).text("Loading...");
    $.ajax({
        url: 'https://api.github.com/repos/' + GH_USER + '/' + GH_REPO + '/releases/latest',
        success: function (data) {
            if (data && data.assets && data.assets.length > 0) {
                var notes = data.body;
                var converter = new Showdown.converter();
                var htmlnotes = converter.makeHtml(notes);
                // Correction for github flavor of markdown, issue references
                htmlnotes = htmlnotes.replace(/<h1>/g, '#').replace('</h1>', '');
                htmlnotes = htmlnotes.replace(/<p>/g, '<br/><br/><p>');
                $("#" + releasenotes).html(htmlnotes);
            } else {
                $("#" + releasenotes).text("");
            }
            if (callback) {
                callback();
            }
        },
        error: function (xhr, options, error) {
            $("#" + releasenotes).text("");
            if (xhr && xhr.status && xhr.status == 403) {
                $("#" + releasenotes).text("You really want FXB, right!?");
            }
            else {
                $("#" + releasenotes).text(error);
            }
        }
    });
};

LoadIssues = function (open, closed) {
    $("#" + open).text("?");
    $("#" + closed).text("?");
    $.ajax({
        url: 'https://api.github.com/repos/' + GH_USER + '/' + GH_REPO + '/issues?state=open',
        success: function (data) {
            var count = 0;
            if (data) {
                count = data.length;
            }
            $("#" + open).text(count);
        }
    });
    $.ajax({
        url: 'https://api.github.com/repos/' + GH_USER + '/' + GH_REPO + '/issues?state=closed',
        success: function (data) {
            var count = 0;
            if (data) {
                count = data.length;
            }
            $("#" + closed).text(count);
        }
    });
};

var sort_by = function (field, reverse, primer) {

    var key = primer ?
        function (x) { return primer(x[field]) } :
        function (x) { return x[field] };

    reverse = !reverse ? 1 : -1;

    return function (a, b) {
        return a = key(a), b = key(b), reverse * ((a > b) - (b > a));
    }
};

Date.prototype.toFormattedString = function (format) {
    /// <summary>
    /// Formats date string dd (date), mm (month), yyyy (year), MM (min), hh (hour), ss (seconds), ms (millisec), APM (AM/PM)
    /// </summary>
    /// <param name="format"></param>
    /// <returns type=""></returns>
    var d = this;
    var f = "";
    f = f + format.replace(/dd|mm|yyyy|MM|hh|ss|ms|APM|\s|\/|\-|,|\./ig, function match() {
        switch (arguments[0]) {
            case "dd":
                var dd = d.getDate();
                return (dd < 10) ? "0" + dd : dd;
            case "mm":
                var mm = d.getMonth() + 1;
                return (mm < 10) ? "0" + mm : mm;
            case "yyyy": return d.getFullYear();
            case "hh":
                var hh = d.getHours();
                return (hh < 10) ? "0" + hh : hh;
            case "MM":
                var MM = d.getMinutes();
                return (MM < 10) ? "0" + MM : MM;
            case "ss":
                var ss = d.getSeconds();
                return (ss < 10) ? "0" + ss : ss;
            case "ms": return d.getMilliseconds();
            case "APM":
                var apm = d.getHours();
                return (apm < 12) ? "AM" : "PM";
            default: return arguments[0];
        }
    } // end match function
    ); // end format.replace
    return f;
};
