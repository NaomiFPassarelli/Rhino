var JQGridExportExcel = {

    Grid: null,
    ColNames: null,
	TotalShownCols: 0,
	ColumnTitles: null,
	FirstID: 0,
	ColumnModel: null,
	NonExportedColumns: ['Predeterminado', 'Acciones', 'Seleccionados'],
    /* Grouping Atributes */
	IsGrouped: false,
	GroupInfo: null,
	GroupCount: 0,
	CurrentGroup: "",
    LastGroup: "",
    /* Footer Atributes */
	HasFooter: false,
	FooterInfo: null,
	DataOfGrid: null,
    /* Treeview Atributes */
	HasTreeView: false,
	TreeColumn: '',
    TreeMaxLevel:0,


	OutputHtml: "",


	Export: function (GridId, Filename) {
	    var html = this.GridToHtmlTable(GridId);
	    $.ExportTableToExcel(html, Filename);
	},


	GridToHtmlTable: function (GridId) {
	    
		this.Grid = $('#' + GridId);
		if (!this.HasData()) return;
		this.GetGridConfig();
		this.LoadDataOfGrid();
		// Armo el HTML
		this.OutputHtml = "<table>";
		this.OutputHtml += this.CreateHeader();
		this.OutputHtml += "<tbody>";
		this.OutputHtml += this.CreateBody();
		if (this.HasFooter) {
			this.OutputHtml += this.CreateFooter();
		}
		this.OutputHtml += "</tbody>";
		return this.OutputHtml;
	},

	CreateHeader: function(){
	    var html = "<thead>";
	    for (j = 0; j < this.ColNames.length; j++) {
	        if (this.ColumnModel[j].Visible && this.NonExportedColumns.indexOf(this.ColNames[j]) == -1) {
	            var colSpan = 1;
	            if (this.HasTreeView && this.TreeColumn == this.ColNames[j]) {
	                colSpan = this.TreeMaxLevel + 1;
	            }
	            html += "<th style='border:1px solid black;' colspan='" + colSpan + "'>" + (this.ColumnTitles[j] != null ? this.ColumnTitles[j] : "") + "</th>";
	            this.TotalShownCols++;
	        }
	    }
	    html += "</thead>";
	    return html;
	},

	CreateBody: function(){
	    var html = "";
	    for (var d = 0; d < this.DataOfGrid.length ; d++) {
	        row = this.DataOfGrid[d];

	        if (this.IsGrouped && this.GroupInfo.groupField) {
	            this.CurrentGroup = this.GroupInfo.groupText[0];
	            for (var x = 0; x < this.GroupInfo.groupField.length; x++) {
	                this.CurrentGroup = this.CurrentGroup.replace('{' + x + '}', row[this.GroupInfo.groupField[x]] != null ? row[this.GroupInfo.groupField[x]] : " ");
	            }
	            var testGroup = this.CurrentGroup.replace('{' + this.GroupInfo.groupField.length + '}', this.GroupCount);
	            if (testGroup != this.LastGroup) {
	                if (this.LastGroup != "") {
                        //TODO -> Imprimir footer del grupo
	                }
	                this.GroupCount = 0;
	                this.CountCurrentGroupRows(d);
	                this.CurrentGroup = this.CurrentGroup.replace('{' + this.GroupInfo.groupField.length + '}', this.GroupCount);
	                html += "<tr>";
	                html += "<td colspan='" + this.TotalShownCols + "' style='border:1px solid black;background-color:rgb(189,215,238);'>" + this.CurrentGroup + "</td>";
	                html += "</tr>";
	            } else {
	                this.CurrentGroup = this.CurrentGroup.replace('{' + this.GroupInfo.groupField.length + '}', this.GroupCount)
	            }
	            this.LastGroup = this.CurrentGroup;
	        }

            /* Print rows - All grid data - Controls & Format Columns */

	        html += "<tr>";
	        for (j = 0; j < this.ColNames.length; j++) {

	            if (this.ColumnModel[j].Visible && this.NonExportedColumns.indexOf(this.ColNames[j]) == -1) {
	                var fieldData = (row[this.ColNames[j]] != null ? row[this.ColNames[j]] : "")
	                if ($.isFunction(this.ColumnModel[j].Formatter)) {
	                    fieldData = this.ColumnModel[j].Formatter.call(this.Grid, fieldData, { colModel: { formatoptions: this.ColumnModel[j].FormatOptions } }, row, null);
	                } else if ($.fmatter) {
	                    fieldData = $.fn.fmatter.call(this.Grid, this.ColumnModel[j].Formatter, fieldData, this.ColumnModel[j].FormatOptions, row, null);
	                }
	                var colSpan = 1;
	                if (this.HasTreeView && this.TreeColumn == this.ColNames[j]) {
	                    colSpan = this.TreeMaxLevel + 1 - row.level;
	                    colEmptySpan = row.level;
	                    if (colEmptySpan > 0) {
	                        html += "<td style='border:1px solid black;border-right:none;' colspan='" + colEmptySpan + "' ></td>";
	                        html += "<td style='border:1px solid black;border-left:none;' colspan='" + colSpan + "'>" + fieldData + "</td>";
	                    }
	                    else {
	                        html += "<td style='border:1px solid black;' colspan='" + colSpan + "'>" + fieldData + "</td>";
	                    }
	                   
	                }
	                else {
	                    html += "<td style='border:1px solid black;'>" + fieldData + "</td>";
	                }
	                
	            }

	        }
	        html += "</tr>";  // output each row with end of line
	    }

	    return html;
	},

	CountCurrentGroupRows:function(startIndex){
	    for (var u = startIndex; u < this.DataOfGrid.length; u++) {
	        var groupCountData = this.DataOfGrid[u];
	        var checkGroup = this.GroupInfo.groupText[0];
	        for (var x = 0; x < this.GroupInfo.groupField.length; x++) {
	            checkGroup = checkGroup.replace('{' + x + '}', groupCountData[this.GroupInfo.groupField[x]] != null ? groupCountData[this.GroupInfo.groupField[x]] : " ");
	        }
	        if (checkGroup == this.CurrentGroup) {
	            this.GroupCount++;
	        }
	        else {
	            break;
	        }
	    }
	},

	CreateFooter: function () {
	    var html = "<tr><tr>"; // Dejo una fila de espacio
	    html += "<tr>";
	    for (j = 0; j < this.ColNames.length; j++) {
	        if (this.ColumnModel[j].Visible && this.NonExportedColumns.indexOf(this.ColNames[j]) == -1) {
	            var fieldData = (this.FooterInfo[this.ColNames[j]] != null ? this.FooterInfo[this.ColNames[j]] : "")
	            if ($.isFunction(this.ColumnModel[j].Formatter)) {
	                fieldData = this.ColumnModel[j].Formatter.call(this.Grid, fieldData, { colModel: { formatoptions: this.ColumnModel[j].FormatOptions } }, this.FooterInfo, null);
	            } else if ($.fmatter) {
	                fieldData = $.fn.fmatter.call(this.Grid, this.ColumnModel[j].Formatter, fieldData, this.ColumnModel[j].FormatOptions, this.FooterInfo, null);
	            }
	            html = html + "<td style='border:1px solid black;'>" + fieldData + "</td>";
	        }
	    }
	    html += "</tr>";
	    return html;
	},

	LoadDataOfGrid: function () {
	    this.DataOfGrid = this.Grid.jqGrid('getGridParam', 'lastSelected');
	    if (this.DataOfGrid == null || typeof this.DataOfGrid === "undefined") {
	        this.DataOfGrid = this.Grid.jqGrid('getGridParam', 'data');
	    }
	    if (this.HasTreeView) {
	        this.GetTreeMaxLevel();
	    }
	},

	GetGridConfig: function(){
		this.IsGrouped = this.Grid.jqGrid('getGridParam', 'grouping');
		if (this.IsGrouped) {
			this.GroupInfo = this.Grid.jqGrid('getGridParam', 'groupingView');
		}
		this.ColumnTitles = this.Grid.jqGrid('getGridParam', 'colNames');
		this.GetColNames();
		this.GetColModel();
		this.HasFooter = this.Grid.jqGrid('getGridParam', 'userDataOnFooter');
		if (this.HasFooter) {
			this.FooterInfo = this.Grid.jqGrid('getGridParam', 'userData');
		}
		this.HasTreeView = this.Grid.jqGrid('getGridParam', 'treeGrid');
		if (this.HasTreeView) {
		    this.TreeColumn = this.Grid.jqGrid('getGridParam', 'ExpandColumn');
		}
	},

	GetColModel:function(){
		// Get ColModel
		var colModel = this.Grid.jqGrid('getGridParam', 'colModel');
		var colModelProxy = [];
		for (var i = 0; i < colModel.length; i++) {
			colModelProxy.push({
				DataField: colModel[i].name,
				Width: colModel[i].width,
				Visible: (colModel[i].hidden == false),
				Formatter: colModel[i].formatter,
				FormatOptions: colModel[i].formatoptions
			});
		}
		this.ColumnModel = colModelProxy;
	},

	GetColNames: function () {
		var colNames = new Array();
		var data = this.Grid.getRowData(this.FirstID);
		var i = 0;
		for (var name in data) { colNames[i++] = name; }
		this.ColNames = colNames;
	},

	HasData: function () {
		var Ids = this.Grid.getDataIDs()
		this.FirstID = Ids.length > 0 ? Ids[0] : 0;
		return Ids.length > 0;
	},

	GetTreeMaxLevel: function () {
	    $.each(this.DataOfGrid, function (i, item) {
	        if (item.level > JQGridExportExcel.TreeMaxLevel) {
	            JQGridExportExcel.TreeMaxLevel = item.level;
	        }
	    });
	},

	GetColLetterFromNumber: function(n) {
        var ordA = 'a'.charCodeAt(0);
        var ordZ = 'z'.charCodeAt(0);
        var len = ordZ - ordA + 1;
      
        var s = "";
        while(n >= 0) {
            s = String.fromCharCode(n % len + ordA) + s;
            n = Math.floor(n / len) - 1;
        }
        return s.toUpperCase();
    }
}